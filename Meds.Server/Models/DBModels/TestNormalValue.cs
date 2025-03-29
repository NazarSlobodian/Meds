using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestNormalValue
{
    public int TestTypeId { get; set; }

    public int MinAge { get; set; }

    public int MaxAge { get; set; }

    public string Gender { get; set; } = null!;

    public decimal MinResValue { get; set; }

    public decimal MaxResValue { get; set; }

    public int TestNormalValueId { get; set; }

    public virtual TestType TestType { get; set; } = null!;
}
