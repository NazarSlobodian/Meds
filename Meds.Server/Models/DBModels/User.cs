using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual Patient? Patient { get; set; }

    public virtual Technician? Technician { get; set; }
}
