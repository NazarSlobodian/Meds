using Meds.Server.Models.DBModels;

public partial class TestNormalValueDTO
{
    public int TestNormalValuesId { get; set; }
    public int TestTypeId { get; set; }

    public int MinAge { get; set; }

    public int MaxAge { get; set; }

    public string Gender { get; set; } = null!;

    public decimal MinResValue { get; set; }

    public decimal MaxResValue { get; set; }

    public TestNormalValueDTO(TestNormalValue tnv)
    {
        TestNormalValuesId = tnv.TestNormalValuesId;
        TestTypeId = tnv.TestTypeId;
        MinAge = tnv.MinAge;
        MaxAge = tnv.MaxAge;
        Gender = tnv.Gender;
        MinResValue = tnv.MinResValue;
        MaxResValue = tnv.MaxResValue;
    }
    public TestNormalValueDTO() { }

}