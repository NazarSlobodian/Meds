using Meds.Server.Models.DbModels;

public class TestBatchLabWorkerDTO
{
    public int TestBatchId {get; set;}
    public DateTime DateOfCreation {get; set;}
    public TestBatchLabWorkerDTO(TestBatch tb)
    {
        TestBatchId = tb.TestBatchId;
        DateOfCreation = tb.DateOfCreation;
    }
    public TestBatchLabWorkerDTO() { }

}