using Meds.Server.Models.DBModels;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class BatchOrderDTO
{
    public string TestName { get; set; }
    public decimal Result { get; set; }
    public string Units { get; set; }
    public string NormalValue { get; set; }
    public BatchOrderDTO(TestOrder to, string sex, int age)
    {
        TestName = to.TestType.TestName;
        Result = to.TestResult.Result;
        Units = to.TestType.MeasurementsUnit;
        TestNormalValue tnv = to.TestType.TestNormalValues.Where(tnv => tnv.Gender == sex && age >= tnv.MinAge && age <= tnv.MaxAge).First();
        NormalValue = tnv.MinResValue + "-" + tnv.MaxResValue;
    }
}