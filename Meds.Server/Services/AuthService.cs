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

    public async Task<RoleData> AutheticateUserAsync(string login, string password)
    {
        User? result = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        if (result == null)
        {
            return null;
        }
        if (result.Role == "admin")
        {
            return new RoleData(result.Role, result.UserId);
        }
        if (result.Role == "patient")
        {
            Patient? pat = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == result.UserId);
            if (pat == null)
            {
                return null;
            }
            else
            {
                result.UserId = pat.PatientId;
            }
        }
        else if (result.Role == "technician")
        {
            Technician? tech = await _context.Technicians.FirstOrDefaultAsync(t => t.UserId == result.UserId);
            if (tech == null)
            {
                return null;
            }
            else
            {
                result.UserId = tech.TechnicianId;
            }
        }
        return new RoleData(result.Role, result.UserId);
    }
}