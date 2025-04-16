using Meds.Server.Models.DbModels;
using Meds.Server.Services;
using Microsoft.EntityFrameworkCore;

public class ActivityLoggerService
{
    private readonly Wv1Context _context;
    public bool On = true;
    public ActivityLoggerService(Wv1Context context)
    {
        _context = context;
    }

    public async Task Log(string action, string description, string actor, string status)
    {
        if (!On) return;
        _context.ActivityLogs.AddAsync(new ActivityLog { Action = action, Actor = actor, Description = description, Status = status });
    }
}