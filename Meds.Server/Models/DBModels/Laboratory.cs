using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class Laboratory
{
    public int LaboratoryId { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public virtual ICollection<Technician> Technicians { get; set; } = new List<Technician>();
}
