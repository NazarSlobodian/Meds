using Humanizer;
using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Meds.Utilities;
using Microsoft.EntityFrameworkCore;
using System.IO;
public class AuthService
{
    private readonly Wv1Context _context;
    public AuthService(Wv1Context context)
    {
        _context = context;
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
}