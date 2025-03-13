
using Meds.Server.Models.DBModels;

public partial class TestBatchDTO
{
    public int TestBatchId { get; set; }
    public string BatchStatus { get; set; } = null!;

    public DateTime DateOfCreation { get; set; }

    public int PatientId { get; set; }

    public int TechnicianId { get; set; }

    public virtual ICollection<TestOrderDTO> TestOrders { get; set; } = new List<TestOrderDTO>();

    public TestBatchDTO(TestBatch tb)
    {
        TestBatchId = tb.TestBatchId;
        BatchStatus = tb.BatchStatus;
        DateOfCreation = tb.DateOfCreation;
        PatientId = tb.PatientId;
        TechnicianId = tb.TechnicianId;
        TestOrders = tb.TestOrders.Select(to=>new TestOrderDTO(to)).ToList();
    }
}
