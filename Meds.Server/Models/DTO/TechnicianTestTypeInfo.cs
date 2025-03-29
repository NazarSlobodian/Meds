using Meds.Server.Models.DbModels;

public class TechnicianTestTypeInfo
{
    public int TestTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }
    public TechnicianTestTypeInfo(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        Name = tt.Name;
        Cost = tt.Cost;
    }
    public TechnicianTestTypeInfo() { }
}