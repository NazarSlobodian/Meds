using Meds.Server.Models.DbModels;
using Meds.Server.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ActivityLoggerService
{
    private readonly Wv1Context _context;
    private readonly IHttpContextAccessor _contextAccessor;
    public bool On = true;
    public ActivityLoggerService(Wv1Context context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _contextAccessor = contextAccessor;
    }

    public async Task Log(string action, string? description, string? actor, string status)
    {
        if (!On) return;

        if (actor == "" || actor == null)
            actor = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role)?.Value ?? "N/A";
        if (description == "" || description == null)
            description = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(u => u.Type == "UserID")?.ToString() ?? "N/A";

        await _context.ActivityLogs.AddAsync(new ActivityLog { Action = action, Actor = actor, Description = description, Status = status });
        await _context.SaveChangesAsync();
    }
}