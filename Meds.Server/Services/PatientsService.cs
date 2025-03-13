using Meds.Server.Models;
using Meds.Server.Models.DBModels;
using Microsoft.EntityFrameworkCore;

public class PatientsService
{
    private readonly DatabaseforkpzContext _context;
    public PatientsService(DatabaseforkpzContext context)
    {
        _context = context;
    }

    public async Task<List<PatientDTO>> GetPatientListWithNameAsync(string patientName)
    {
        List<Patient> tbs = await _context.Patients.Where(tb => tb.FullName.Contains(patientName)).ToListAsync();
        return tbs.Select(tb => new PatientDTO(tb)).ToList();
    }
    public async Task<List<PatientDTO>> GetPatientListWithNumberAsync(string phoneNumber)
    {
        List<Patient> tbs = await _context.Patients.Where(tb => tb.ContactNumber.Contains(phoneNumber)).ToListAsync();
        return tbs.Select(tb => new PatientDTO(tb)).ToList();
    }
    public async Task<List<TestBatchDTO>> GetPatientBatchesAsync(int patientId)
    {
        List<TestBatch> tbs = await _context.TestBatches.Where(tb => tb.PatientId == patientId).ToListAsync();
        return tbs.Select(tb => new TestBatchDTO(tb)).ToList();
    }
    public async Task<int> AddPatient(PatientNew patient)
    {
        Patient p = new Patient()
        {
            FullName = patient.FullName,
            Gender = patient.Gender,
            DateOfBirth = patient.DateOfBirth,
            Email = patient.Email,
            ContactNumber = patient.PhoneNumber,

        };
        await _context.Patients.AddAsync(p);
        await _context.SaveChangesAsync();
        return p.PatientId;
    }
    public async Task<BatchResultsDTO> GetBatchResultsAsync(int batchId)
    {
        TestBatch batch = await _context.TestBatches
            .Include(tb => tb.Patient)
            .Include(tb => tb.TestOrders)
                .ThenInclude(to => to.TestResult)
            .Include(tb => tb.TestOrders)
                .ThenInclude(to => to.TestType)
                    .ThenInclude(tt => tt.TestNormalValues)
            .Where(tb => tb.TestBatchId == batchId)
            .FirstAsync();
        Laboratory lab = await _context.Laboratories
            .Include(l => l.Technicians)
            .Where(t => t.Technicians.Any(t => t.TechnicianId == batch.TechnicianId))
            .FirstAsync();

        return new BatchResultsDTO(lab, batch);
    }
    public async Task ValidateAndSubmitBatchesAsync(int patientId, int technicianId,List<TechnicianTestTypeInfo> tests)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null)
        {
            throw new ArgumentException("Patient not found.");
        }
        if (tests == null || tests.Count == 0)
        {
            throw new ArgumentException("Test types cannot be empty.");
        }
        var availableTestTypes = await _context.TestTypes.ToListAsync();
        foreach (var testType in tests)
        {
            var existingTestType = availableTestTypes.FirstOrDefault(tt => tt.TestName == testType.TestName && tt.TestTypeId == testType.TestTypeId && tt.Cost == testType.Cost);
            if (existingTestType == null)
            {
                throw new ArgumentException($"Test type '{testType.TestName}' does not exist.");
            }
        }
        var batch = new TestBatch
        {
            PatientId = patientId,
            TechnicianId = technicianId,
            TestOrders = new List<TestOrder>()
        };
        foreach (var testType in tests)
        {
            var testOrder = new TestOrder
            {
                TestTypeId = testType.TestTypeId,
                TestBatchId = batch.TestBatchId 
            };

            batch.TestOrders.Add(testOrder);
        }
       _context.TestBatches.Add(batch);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Couldn't add to db {ex.Message}");
        }
    }
}