using Meds.Server.Models.DbModels;

public class AdminTestPanelInfo
{
    public int TestPanelId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public AdminTestPanelInfo(TestPanel tp)
    {
        TestPanelId = tp.TestPanelId;
        Name = tp.Name;
        Cost = tp.Cost;
    }
    public AdminTestPanelInfo() { }
}