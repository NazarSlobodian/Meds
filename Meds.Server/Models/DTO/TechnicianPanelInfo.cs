
using Meds.Server.Models.DbModels;

public class TechnicianPanelInfo
{
    public int TestPanelId { get; set; }
    public List<string> Contents { get; set; } = new List<string>();
    public string Name { get; set; }
    public decimal Cost { get; set; }

    public TechnicianPanelInfo(TestPanel panel)
    {
        TestPanelId = panel.TestPanelId;
        Contents = panel.TestTypes.Select(x => x.Name).ToList();
        Name = panel.Name;
        Cost = panel.Cost;

    }
    public TechnicianPanelInfo() { }
}
