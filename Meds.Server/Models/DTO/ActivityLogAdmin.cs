using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DbModels;

public partial class ActivityLogAdmin
{

    public string Action { get; set; }

    public string Description { get; set; }

    public string Actor { get; set; }

    public string Status { get; set; }
    public DateTime DateTime { get; set; }

}
