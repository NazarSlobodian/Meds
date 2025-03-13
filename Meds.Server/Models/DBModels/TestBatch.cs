using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class TestBatch
{
    public int TestBatchId { get; set; }

    public string BatchStatus { get; set; } = null!;

    public DateTime DateOfCreation { get; set; }

    public int PatientId { get; set; }

    public int TechnicianId { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Technician Technician { get; set; } = null!;

    public virtual ICollection<TestOrder> TestOrders { get; set; } = new List<TestOrder>();
}
