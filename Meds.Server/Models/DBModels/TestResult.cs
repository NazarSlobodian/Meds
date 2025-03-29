using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestResult
{
    public int TestOrderId { get; set; }

    public decimal Result { get; set; }

    public DateOnly DateOfTest { get; set; }

    public virtual TestOrder TestOrder { get; set; } = null!;
}
