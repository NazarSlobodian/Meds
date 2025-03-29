using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class Receptionist
{
    public int ReceptionistId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContectNumber { get; set; } = null!;

    public int CollectionPointId { get; set; }

    public virtual CollectionPoint CollectionPoint { get; set; } = null!;

    public virtual ICollection<TestBatch> TestBatches { get; set; } = new List<TestBatch>();
}
