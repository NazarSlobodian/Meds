using Meds.Server.Models.DBModels;

public partial class TestResultDTO
{
    public int TestOrderId { get; set; }

    public decimal Result { get; set; }

    public DateOnly DateOfTest { get; set; }

    public TestResultDTO(TestResult tr)
    {
        TestOrderId = tr.TestOrderId;
        Result = tr.Result;
        DateOfTest = tr.DateOfTest;
    }
}
