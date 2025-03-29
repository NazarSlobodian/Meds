using Meds.Server.Models.DbModels;

public class TechnicianTestTypeInfo
{
    public int TestTypeId { get; set; }

    public string TestName { get; set; } = null!;

    public decimal Cost { get; set; }
    public TechnicianTestTypeInfo(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        TestName = tt.Name;
        Cost = tt.Cost;
    }
    public TechnicianTestTypeInfo() { }
}