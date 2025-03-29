using Meds.Server.Models.DbModels;

public class AdminTestTypeInfo
{
    public int TestTypeId { get; set; }

    public string TestName { get; set; } = null!;

    public decimal Cost { get; set; }
    public int DaysTillOverdue { get; set; }
    public string MeasurementsUnit { get; set; }

    public AdminTestTypeInfo(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        TestName = tt.Name;
        Cost = tt.Cost;
        DaysTillOverdue = tt.DaysTillOverdue;
        MeasurementsUnit = tt.MeasurementsUnit;
    }
    public AdminTestTypeInfo() { }
}