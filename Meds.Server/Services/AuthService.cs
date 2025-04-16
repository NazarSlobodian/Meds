using Humanizer;
using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Meds.Server.Services;
using Meds.Utilities;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IO;
public class AuthService
{
    private readonly Wv1Context _context;
    private readonly MailService _mailService;
    public AuthService(Wv1Context context, MailService mailService)
    {
        _context = context;
        _mailService = mailService;
    }

    public async Task<RoleData> AutheticateUserAsync(string login, string password)
    {
        User? result = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (result == null)
        {
            return null;
        }
        string hash = PasswordHasher.HashPassword(password);
        bool match = PasswordHasher.VerifyPassword(password, result.Hash);
        if (match)
        {
            return new RoleData(result.Role, result.ReferencedId);
        }
        else
        {
            return null;
        }
    }
    public async Task InitRegistration(string email)
    {
        Patient? patient = await _context.Patients.Where(x => x.Email == email).FirstOrDefaultAsync();
        if (patient == null)
        {
            throw new Exception("Email isn't associated with any client. Contact tech support or reception desk and provide your email");
        }
        Random random = new Random();
        RegistrationCode? registrationCode = await _context.RegistrationCodes.FindAsync(email);
        int code = random.Next(111111, 1000000);
        string codeStr = code.ToString();
        if (registrationCode == null)
        {
            await _context.RegistrationCodes.AddAsync(new RegistrationCode { Code = codeStr, Login = email });
        }
        else
        {
            registrationCode.Code = code.ToString();
        }
        await _context.SaveChangesAsync();
        //await _mailService.SendCode(email, codeStr);
    }
    public async Task<bool> VerifyCode(string email, string code)
    {
        RegistrationCode? registrationCode = await _context.RegistrationCodes.FindAsync(email);
        if (registrationCode == null)
        {
            throw new Exception("Code not found");
        }
        if (registrationCode.Code == code)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public async Task FinishRegistration(string login, string password)
    {
        Patient? patient = await _context.Patients.Where(x => x.Email == login).FirstOrDefaultAsync();
        if (patient == null)
        {
            throw new Exception("Email isn't associated with any client. Contact tech support or reception desk and provide your email");
        }
        string hash = PasswordHasher.HashPassword(password);
        await _context.Users.AddAsync(new User { Hash = hash, Login = login, Role = "patient", ReferencedId = patient.PatientId });
        await _context.SaveChangesAsync();
    }
}