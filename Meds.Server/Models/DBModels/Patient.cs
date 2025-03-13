using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class Patient
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string? ContactNumber { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<TestBatch> TestBatches { get; set; } = new List<TestBatch>();

    public virtual User? User { get; set; }
}
