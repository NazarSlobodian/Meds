using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class RegistrationCode
{
    public string Login { get; set; } = null!;

    public string Code { get; set; } = null!;
}
