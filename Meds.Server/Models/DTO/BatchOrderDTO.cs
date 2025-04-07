using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class BatchOrderDTO
{
    public string Name { get; set; }
    public decimal Result { get; set; }
    public string Units { get; set; }
    public string NormalValue { get; set; }
    public BatchOrderDTO(TestOrder to, string sex, int age)
    {
        Name = to.TestType.Name;
        Result = to.TestResult.Result;
        Units = to.TestType.MeasurementsUnit;
        try
        {
            TestNormalValue? tnv = to.TestType.TestNormalValues.Where(tnv => tnv.Gender == sex && age >= tnv.MinAge && age <= tnv.MaxAge).First();
            NormalValue = tnv.MinResValue + "-" + tnv.MaxResValue;
        }
        catch
        {
            NormalValue = "N/A";
        }
    }
}