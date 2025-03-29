using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestBatch
{
    public int TestBatchId { get; set; }

    public string BatchStatus { get; set; } = null!;

    public DateTime DateOfCreation { get; set; }

    public int PatientId { get; set; }

    public int ReceptionistId { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Receptionist Receptionist { get; set; } = null!;

    public virtual ICollection<TestOrder> TestOrders { get; set; } = new List<TestOrder>();
}
