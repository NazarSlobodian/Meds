using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class TestPanel
{
    public int TestPanelId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public sbyte IsActive { get; set; }

    public virtual ICollection<TestOrder> TestOrders { get; set; } = new List<TestOrder>();

    public virtual ICollection<TestType> TestTypes { get; set; } = new List<TestType>();
}
