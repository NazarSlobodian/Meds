using Meds.Server.Models.DbModels;

public class AdminTestTypeInfo
{
    public int TestTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }
    public string MeasurementsUnit { get; set; }
    public bool IsActive { get; set; }
    public AdminTestTypeInfo(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        Name = tt.Name;
        Cost = tt.Cost;
        MeasurementsUnit = tt.MeasurementsUnit;
        IsActive = (tt.IsActive == 1) ? true : false;
    }
    public AdminTestTypeInfo() { }
}