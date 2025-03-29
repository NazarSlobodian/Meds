using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestOrder
{
    public int TestOrderId { get; set; }

    public int TestTypeId { get; set; }

    public int TestBatchId { get; set; }

    public int? TestPanelId { get; set; }

    public int LaboratoryId { get; set; }

    public virtual Laboratory Laboratory { get; set; } = null!;

    public virtual TestBatch TestBatch { get; set; } = null!;

    public virtual TestPanel? TestPanel { get; set; }

    public virtual TestResult? TestResult { get; set; }

    public virtual TestType TestType { get; set; } = null!;
}
