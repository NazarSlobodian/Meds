using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class User
{
    public int UsersId { get; set; }

    public string Role { get; set; } = null!;

    public int ReferencedId { get; set; }

    public string Login { get; set; } = null!;

    public string Hash { get; set; } = null!;
}
