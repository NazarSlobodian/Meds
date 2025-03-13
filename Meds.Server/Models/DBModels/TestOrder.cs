using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class TestOrder
{
    public int TestOrderId { get; set; }

    public int TestTypeId { get; set; }

    public int TestBatchId { get; set; }

    public virtual TestBatch TestBatch { get; set; } = null!;

    public virtual TestResult? TestResult { get; set; }

    public virtual TestType TestType { get; set; } = null!;
}
