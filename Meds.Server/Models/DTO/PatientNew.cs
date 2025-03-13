using Meds.Server.Models.DBModels;
using System.ComponentModel.DataAnnotations;

public class PatientNew
{
    public string FullName { get; set; }
    public string Gender { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}