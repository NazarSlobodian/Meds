using Humanizer;
using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class AuthService
{
    private readonly Wv1Context _context;
    public AuthService(Wv1Context context)
    {
        _context = context;
    }

    public async Task<RoleData> AutheticateUserAsync(string login, string hash)
    {
        User? result = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Hash == hash);
        if (result == null)
        {
            return null;
        }
        return new RoleData(result.Role, result.ReferencedId);
    }
}