
using Meds.Server.Models.DBModels;

public class BatchResultsDTO
{
    public String LabAddress { get; set; }
    public String Email { get; set; }
    public String Phone { get; set; }
    public int BatchID { get; set; }
    public DateTime TimeOfCreation { get; set; }
    public String PatientName { get; set; }
    public String PatientSex { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public List<BatchOrderDTO> TestResults { get; set; }
    public BatchResultsDTO(Laboratory lab, TestBatch tb)
    {
        LabAddress = lab.Address;
        Email = lab.Email;
        Phone = lab.ContactNumber;
        BatchID = tb.TestBatchId;
        TimeOfCreation = tb.DateOfCreation;
        PatientName = tb.Patient.FullName;
        PatientSex = tb.Patient.Gender;
        DateOfBirth = tb.Patient.DateOfBirth;
        TestResults = new List<BatchOrderDTO>();
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        foreach (TestOrder to in tb.TestOrders)
        {
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth > today.AddYears(-age))
                age--;

            TestResults.Add(new BatchOrderDTO(to, PatientSex, age));
        }
    }
}
