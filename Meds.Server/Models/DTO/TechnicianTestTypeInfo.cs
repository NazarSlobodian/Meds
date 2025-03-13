using Meds.Server.Models.DBModels;

public class TechnicianTestTypeInfo
{
    public int TestTypeId { get; set; }

    public string TestName { get; set; } = null!;

    public decimal Cost { get; set; }
    public TechnicianTestTypeInfo(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        TestName = tt.TestName;
        Cost = tt.Cost;
    }
    public TechnicianTestTypeInfo() { }
}