using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class Technician
{
    public int TechnicianId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public int LaboratoryId { get; set; }

    public int UserId { get; set; }

    public virtual Laboratory Laboratory { get; set; } = null!;

    public virtual ICollection<TestBatch> TestBatches { get; set; } = new List<TestBatch>();

    public virtual User User { get; set; } = null!;
}
