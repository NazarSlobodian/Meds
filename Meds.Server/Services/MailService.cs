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
namespace MailGunExamples
{
    class MailService
    {
        private readonly Wv1Context _context;
        public MailService(Wv1Context context)
        {
            _context = context;
        }

        public static async Task<RestResponse> Send()
        {
            var options = new RestClientOptions("https://api.mailgun.net")
            {
                Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("API_KEY") ?? "API_KEY")
            };

            var client = new RestClient(options);
            var request = new RestRequest("/v3/sandbox7d22d6dfd8e24bcfb293d7cfacfccbaf.mailgun.org/messages", RestSharp.Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox7d22d6dfd8e24bcfb293d7cfacfccbaf.mailgun.org>");
            request.AddParameter("to", "Nazar <nazar.slobodian.pz.2022@lpnu.ua>");
            request.AddParameter("subject", "Hello Nazar");
            request.AddParameter("text", "Congratulations Nazar, you just sent an email with Mailgun! You are truly awesome!");
            return await client.ExecuteAsync(request);
        }
        public static byte[] MakePdfResult(BatchResultsDTO dto)
        {
            var grouped = dto.TestResults
                .GroupBy(t => t.PanelName ?? "Other")
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
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text($"Lab Address: {dto.LabAddress}");
                                left.Item().Text($"Email: {dto.Email}");
                                left.Item().Text($"Phone: {dto.Phone}");
                                left.Item().Text($"Batch ID: {dto.BatchID}");
                                left.Item().Text($"Date: {dto.TimeOfCreation:yyyy-MM-dd HH-mm}");
                            });

                            row.RelativeItem().Column(right =>
                            {
                                right.Item().AlignRight().Text($"Patient: {dto.PatientName}");
                                right.Item().AlignRight().Text($"Sex: {dto.PatientSex}");
                                right.Item().AlignRight().Text($"Date of birth: {dto.DateOfBirth}");
                            });
                        });
                        col.Item().LineHorizontal(1).LineColor("#211446");

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
                                    row.RelativeItem(1).AlignRight().Text($"{test.Units}");
                                });
                            }
                        }
                        col.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor("#211446");
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
            string filePath = "d:\\Downloads\\";
            File.WriteAllBytes(filePath, pdfBytes);
        }
    }
}
