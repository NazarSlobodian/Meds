using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class Laboratory
{
    public int LaboratoryId { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public virtual ICollection<LabWorker> LabWorkers { get; set; } = new List<LabWorker>();

    public virtual ICollection<TestOrder> TestOrders { get; set; } = new List<TestOrder>();

    public virtual ICollection<TestType> TestTypes { get; set; } = new List<TestType>();
}
