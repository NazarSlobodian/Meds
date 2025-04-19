using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestResult
{
    public int TestOrderId { get; set; }

    public decimal Result { get; set; }

    public DateTime DateOfTest { get; set; }

    public int LabWorkerId { get; set; }

    public virtual LabWorker LabWorker { get; set; } = null!;

    public virtual TestOrder TestOrder { get; set; } = null!;
}
