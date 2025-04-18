using Humanizer;
using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Meds.Server.Services;
using Meds.Utilities;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IO;
public class AuthService
{
    private readonly Wv1Context _context;
    private readonly MailService _mailService;
    private readonly ActivityLoggerService _activityLoggerService;
    public AuthService(Wv1Context context, MailService mailService, ActivityLoggerService activityLoggerService)
    {
        _context = context;
        _mailService = mailService;
        _activityLoggerService = activityLoggerService;
    }

    public async Task<RoleData> AutheticateUserAsync(string login, string password)
    {
        User? result = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (result == null)
        {
            await _activityLoggerService.Log("Log in", "Login doesn't exist", "Guest", "fail");
            return null;
        }
        string hash = PasswordHasher.HashPassword(password);
        bool match = PasswordHasher.VerifyPassword(password, result.Hash);
        if (match)
        {
            await _activityLoggerService.Log("Log in", $"{login}", $"{result.Role}", "success");
            return new RoleData(result.Role, result.ReferencedId);
        }
        else
        {
            await _activityLoggerService.Log("Log in", $"{login}", $"{result.Role}", "fail");
            return null;
        }
    }
    public async Task InitRegistration(string email)
    {
        Patient? patient = await _context.Patients.Where(x => x.Email == email).FirstOrDefaultAsync();
        if (patient == null)
        {
            await _activityLoggerService.Log("Code request", "Login doesn't exist", "guest", "fail");
            throw new Exception("Email isn't associated with any client. Contact tech support or reception desk and provide your email");
        }
        User? user = await _context.Users.Where(u => u.Login == email).FirstOrDefaultAsync();
        if (user != null)
        {
            await _activityLoggerService.Log("Code request", "User already exists", "guest", "fail");
            throw new Exception("You are already registered");
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
        await _activityLoggerService.Log("Code save", $"{email}", "guest", "success");
        await _mailService.SendCode(email, codeStr);

    }
    public async Task<bool> VerifyCode(string email, string code)
    {
        RegistrationCode? registrationCode = await _context.RegistrationCodes.FindAsync(email);
        if (registrationCode == null)
        {
            await _activityLoggerService.Log("Code verification", $"{email}", "guest", "fail");
            throw new Exception("Code not found");
        }
        if (registrationCode.Code == code)
        {
            await _activityLoggerService.Log("Code verification", $"{email}", "guest", "success");
            return true;
        }
        else
        {
            await _activityLoggerService.Log("Code verification", $"{email}", "guest", "fail");
            return false;
        }
    }
    public async Task FinishRegistration(string login, string password)
    {
        Patient? patient = await _context.Patients.Where(x => x.Email == login).FirstOrDefaultAsync();
        if (patient == null)
        {
            await _activityLoggerService.Log("Account registration", $"{login}", "guest", "fail");
            throw new Exception("Email isn't associated with any client. Contact tech support or reception desk and provide your email");
        }
        string hash = PasswordHasher.HashPassword(password);
        await _context.Users.AddAsync(new User { Hash = hash, Login = login, Role = "patient", ReferencedId = patient.PatientId });
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Account registration", $"{login}", "guest", "success");
    }
}