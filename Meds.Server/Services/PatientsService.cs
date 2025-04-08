using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;

public class PatientsService
{
    private readonly Wv1Context _context;
    public PatientsService(Wv1Context context)
    {
        _context = context;
    }

    public async Task<ListWithTotalCount<PatientDTO>> GetPatientListPagedAsync(string? name, string? phone, string? email, string? dateOfBirth, int page, int pageSize)
    {
        string? trimmedName = name?.Trim();
        string? trimmedPhone = phone?.Trim();
        string? trimmedEmail = email?.Trim();
        string? trimmedDob = dateOfBirth?.Trim();

        var query = _context.Patients.AsQueryable();
        if (!string.IsNullOrWhiteSpace(trimmedName))
        {
            query = query.Where(p => p.FullName.Contains(trimmedName));
        }
        if (!string.IsNullOrWhiteSpace(trimmedPhone))
        {
            query = query.Where(p => p.ContactNumber != null && p.ContactNumber.Contains(trimmedPhone));
        }
        if (!string.IsNullOrWhiteSpace(trimmedEmail))
        {
            query = query.Where(p => p.Email != null && p.Email.Contains(trimmedEmail));
        }
        if (!string.IsNullOrWhiteSpace(trimmedDob))
        {
            if (DateOnly.TryParse(trimmedDob, out DateOnly date))
                query = query.Where(p => p.DateOfBirth == date);
            else
                return new ListWithTotalCount<PatientDTO>();
        }
        List<Patient> tbs = await query.OrderBy(p=>p.FullName).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
        int count = await query.CountAsync();
        return new ListWithTotalCount<PatientDTO>(tbs.Select(tb => new PatientDTO(tb)).ToList(), count);
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
        CollectionPoint collectionPoint = await _context.CollectionPoints
            .Include(l => l.Receptionists)
            .Where(t => t.Receptionists.Any(t => t.ReceptionistId == batch.ReceptionistId))
            .FirstAsync();

        return new BatchResultsDTO(collectionPoint, batch);
    }
    public async Task ValidateAndSubmitBatchesAsync(int patientId, int receptionistId,List<TechnicianTestTypeInfo> tests)
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
            var existingTestType = availableTestTypes.FirstOrDefault(tt => tt.Name == testType.Name && tt.TestTypeId == testType.TestTypeId && tt.Cost == testType.Cost);
            if (existingTestType == null)
            {
                throw new ArgumentException($"Test type '{testType.Name}' does not exist.");
            }
        }
        var batch = new TestBatch
        {
            PatientId = patientId,
            ReceptionistId = receptionistId,
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