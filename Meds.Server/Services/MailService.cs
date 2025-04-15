using System;
using RestSharp; // RestSharp v112.1.0
using RestSharp.Authenticators;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Meds.Server.Models.DbModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using Microsoft.OpenApi.Validations;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
namespace MailGunExamples
{
    public class MailService
    {
        private readonly Wv1Context _context;
        public MailService(Wv1Context context)
        {
            _context = context;
           
        }

        public static async Task Send(BatchResultsDTO dto)
        {
            var options = new RestClientOptions("https://api.mailgun.net")
            {
                Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("API_KEY") ?? "API_KEY")
            };

            var client = new RestClient(options);
            var request = new RestRequest("/v3/sandbox7d22d6dfd8e24bcfb293d7cfacfccbaf.mailgun.org/messages", RestSharp.Method.Post);
            request.AlwaysMultipartFormData = true;

            // Basic email info
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox7d22d6dfd8e24bcfb293d7cfacfccbaf.mailgun.org>");
            request.AddParameter("to", "Nazar <nazar.slobodian.pz.2022@lpnu.ua>");
            request.AddParameter("subject", "Medlab results");
            request.AddParameter("text", "Thank you for using our services! Your report is attached as PDF.");

            // Generate and attach PDF
            var pdfBytes = MakePdfResult(dto);
            request.AddFile("attachment", pdfBytes, $"Medlab{dto.BatchID}.pdf", "application/pdf");

            await client.ExecuteAsync(request);
        }

        public static byte[] MakePdfResult(BatchResultsDTO dto)
        {
            var grouped = dto.TestResults
                .GroupBy(t => t.PanelName ?? "Other")
                .OrderBy(g => g.Key, Comparer<string>.Create((a, b) =>
                {
                    if (a == "Other") return 1;
                    if (b == "Other") return -1;
                    return string.Compare(a, b);
                }))
                .ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content()
                    .Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(1).AlignLeft().Text("Medlab");
                        });
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text($"Lab Address: {dto.LabAddress}");
                                left.Item().Text($"Email: {dto.Email}");
                                left.Item().Text($"Phone: {dto.Phone}");
                                left.Item().Text($"Batch ID: {dto.BatchID}");
                                left.Item().Text($"Date: {dto.TimeOfCreation:yyyy-MM-dd HH:mm}");
                            });

                            row.RelativeItem().Column(right =>
                            {
                                right.Item().AlignRight().Text($"Patient: {dto.PatientName}");
                                right.Item().AlignRight().Text($"Sex: {dto.PatientSex}");
                                right.Item().AlignRight().Text($"Date of birth: {dto.DateOfBirth}");
                            });
                        });
                        col.Item().LineHorizontal(2).LineColor("#211446");
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(1).AlignLeft().Text("Test name");
                            row.RelativeItem(1).AlignCenter().Text("Result");
                            row.RelativeItem(1).AlignCenter().Text("Normal values");
                            row.RelativeItem(1).AlignRight().Text("Units");
                        });
                        foreach (var group in grouped)
                        {
                            col.Item().PaddingTop(10).Text(group.Key).Bold().FontSize(14);
                            foreach (var test in group)
                            {
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem(1).AlignLeft().Text(test.Name);
                                    row.RelativeItem(1).AlignCenter().Text($"{test.Result}");
                                    row.RelativeItem(1).AlignCenter().Text($"{test.NormalValue}");
                                    row.RelativeItem(1).AlignRight().Text($"{ToSuperscript(test.Units)}");
                                });
                            }
                        }
                        col.Item().PaddingVertical(5).LineHorizontal(1.0f).LineColor("#211446");
                    });
                });
            });
            using MemoryStream stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
        public async Task SendResultsAndSave(BatchResultsDTO dto)
        {
            byte[] pdfBytes = MakePdfResult(dto);
            string filePath = $"d:\\Downloads\\{dto.BatchID}.pdf";
            File.WriteAllBytes(filePath, pdfBytes);
            await Send(dto);
        }
        private static string ToSuperscript(string input)
        {
            var superscriptMap = new Dictionary<char, char>
            {
                ['0'] = '⁰',
                ['1'] = '¹',
                ['2'] = '²',
                ['3'] = '³',
                ['4'] = '⁴',
                ['5'] = '⁵',
                ['6'] = '⁶',
                ['7'] = '⁷',
                ['8'] = '⁸',
                ['9'] = '⁹',
                ['+'] = '⁺',
                ['-'] = '⁻',
                ['='] = '⁼',
                ['('] = '⁽',
                [')'] = '⁾'
            };

            var result = new StringBuilder();
            bool inSuperscript = false;

            foreach (char c in input)
            {
                if (c == '^')
                {
                    inSuperscript = true;
                }
                else if (inSuperscript && superscriptMap.ContainsKey(c))
                {
                    result.Append(superscriptMap[c]);
                    inSuperscript = false;
                }
                else
                {
                    result.Append(c);
                    inSuperscript = false;
                }
            }

            return result.ToString();
        }
    }
}
