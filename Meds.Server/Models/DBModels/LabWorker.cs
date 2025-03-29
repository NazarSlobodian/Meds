using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class LabWorker
{
    public int LabWorkerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public int LaboratoryId { get; set; }

    public virtual Laboratory Laboratory { get; set; } = null!;
}
