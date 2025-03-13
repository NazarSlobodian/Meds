using Meds.Server.Models.DBModels;

public partial class TestTypeDTO
{
    public int TestTypeId { get; set; }

    public string TestName { get; set; } = null!;

    public decimal Cost { get; set; }

    public int DaysTillOverdue { get; set; }

    public string MeasurementsUnit { get; set; } = null!;

    public virtual ICollection<TestNormalValueDTO> TestNormalValues { get; set; } = new List<TestNormalValueDTO>();

    public TestTypeDTO(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        TestName = tt.TestName;
        Cost = tt.Cost;
        DaysTillOverdue = tt.DaysTillOverdue;
        MeasurementsUnit = tt.MeasurementsUnit;
        TestNormalValues = tt.TestNormalValues.Select(t=> new TestNormalValueDTO(t)).ToList();
    }

}