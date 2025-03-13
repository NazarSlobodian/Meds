using Meds.Server.Models.DBModels;

public class PatientDTO
{
    public int PatientID {  get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? Email { get; set;}
    public string? PhoneNumber { get; set;}
    public PatientDTO(Patient pat)
    {
        PatientID = pat.PatientId;
        FullName = pat.FullName;
        Gender = pat.Gender;
        DateOfBirth = pat.DateOfBirth;
        Email = pat.Email;
        PhoneNumber = pat.ContactNumber;
    }
} 