using System;
using System.Collections.Generic;

namespace Meds.Server.Models.DBModels;

public partial class Lvivstat
{
    public string Lab { get; set; } = null!;

    public string? Period { get; set; }

    public long TestTaken { get; set; }

    public decimal? Income { get; set; }

    public decimal? Delta { get; set; }
}
