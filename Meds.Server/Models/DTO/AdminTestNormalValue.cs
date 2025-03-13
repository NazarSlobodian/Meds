using Meds.Server.Models.DBModels;

public partial class AdminTestNormalValue
{
    public int TestNormalValuesId { get; set; }

    public int MinAge { get; set; }

    public int MaxAge { get; set; }

    public string Gender { get; set; } = null!;

    public decimal MinResValue { get; set; }

    public decimal MaxResValue { get; set; }

    public AdminTestNormalValue() { }

}