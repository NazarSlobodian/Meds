
using Meds.Server.Models.DBModels;

public partial class TestOrderDTO
{
    public int TestOrderId { get; set; }

    public int TestTypeId { get; set; }

    public int TestBatchId { get; set; }

    public virtual TestResultDTO? TestResult { get; set; }

    public virtual TestTypeDTO TestType { get; set; } = null!;

    public TestOrderDTO(TestOrder to)
    {
        TestOrderId = to.TestOrderId;
        TestTypeId = to.TestTypeId;
        TestBatchId = to.TestBatchId;
        if (to.TestResult == null)
            TestResult = null;
        else
            TestResult = new TestResultDTO(to.TestResult);
        TestType = new TestTypeDTO(to.TestType);
    }
}