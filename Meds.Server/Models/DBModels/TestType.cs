using System;
using System.Collections.Generic;

namespace Scaff.Server.Models.DbModels;

public partial class TestType
{
    public int TestTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public int DaysTillOverdue { get; set; }

    public string MeasurementsUnit { get; set; } = null!;

    public virtual ICollection<TestNormalValue> TestNormalValues { get; set; } = new List<TestNormalValue>();

    public virtual ICollection<TestOrder> TestOrders { get; set; } = new List<TestOrder>();

    public virtual ICollection<TestPanel> TestPanels { get; set; } = new List<TestPanel>();
}
