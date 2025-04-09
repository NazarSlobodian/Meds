
using Meds.Server.Models.DbModels;

public class TechnicianPanelInfo
{
    public List<string> Contents { get; set; } = new List<string>();
    public string Name { get; set; }
    public decimal Cost { get; set; }

    public TechnicianPanelInfo(TestPanel panel)
    {
        Contents = panel.TestTypes.Select(x => x.Name).ToList();
        Name = panel.Name;
        Cost = panel.Cost;

    }
    public TechnicianPanelInfo() { }
}
