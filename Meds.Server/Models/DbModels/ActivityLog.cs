using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class ActivityLog
{
    public int ActivityLogId { get; set; }

    public string Action { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Actor { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public string Status { get; set; } = null!;
}
