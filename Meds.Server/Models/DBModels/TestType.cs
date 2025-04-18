using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestType
{
    public int TestTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public string MeasurementsUnit { get; set; } = null!;

    public sbyte IsActive { get; set; }

    public virtual ICollection<TestNormalValue> TestNormalValues { get; set; } = new List<TestNormalValue>();

    public virtual ICollection<TestOrder> TestOrders { get; set; } = new List<TestOrder>();

    public virtual ICollection<Laboratory> Laboratories { get; set; } = new List<Laboratory>();

    public virtual ICollection<TestPanel> TestPanels { get; set; } = new List<TestPanel>();
}
