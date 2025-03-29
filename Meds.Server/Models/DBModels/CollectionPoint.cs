using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class CollectionPoint
{
    public int CollectionPointId { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public virtual ICollection<Receptionist> Receptionists { get; set; } = new List<Receptionist>();
}
