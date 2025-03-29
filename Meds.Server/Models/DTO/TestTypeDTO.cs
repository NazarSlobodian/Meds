using Meds.Server.Models.DbModels;

public partial class TestTypeDTO
{
    public int TestTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public int DaysTillOverdue { get; set; }

    public string MeasurementsUnit { get; set; } = null!;

    public virtual ICollection<TestNormalValueDTO> TestNormalValues { get; set; } = new List<TestNormalValueDTO>();

    public TestTypeDTO(TestType tt)
    {
        TestTypeId = tt.TestTypeId;
        Name = tt.Name;
        Cost = tt.Cost;
        DaysTillOverdue = tt.DaysTillOverdue;
        MeasurementsUnit = tt.MeasurementsUnit;
        TestNormalValues = tt.TestNormalValues.Select(t=> new TestNormalValueDTO(t)).ToList();
    }

}