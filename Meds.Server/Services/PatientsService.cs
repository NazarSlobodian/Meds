using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

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
        List<Patient> tbs = await query.OrderBy(p => p.FullName).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
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
            .Include(tb => tb.TestOrders)
                .ThenInclude(to => to.TestPanel)
            .Where(tb => tb.TestBatchId == batchId)
            .FirstAsync();
        CollectionPoint collectionPoint = await _context.CollectionPoints
            .Include(l => l.Receptionists)
            .Where(t => t.Receptionists.Any(t => t.ReceptionistId == batch.ReceptionistId))
            .FirstAsync();

        return new BatchResultsDTO(collectionPoint, batch);
    }
    public async Task ValidateAndSubmitBatchesAsync(int patientId, int receptionistId, NewOrder tests)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null)
        {
            throw new ArgumentException("Patient not found.");
        }
        if (tests == null || (tests.TestTypesIds.Count == 0 && tests.PanelsIds.Count == 0))
        {
            throw new ArgumentException("Can't add an empty order.");
        }
        var availableTestTypes = await _context.TestTypes.Select(x => x.TestTypeId).ToListAsync();
        foreach (int testTypeId in tests.TestTypesIds)
        {
            var existingTestType = availableTestTypes.FirstOrDefault(tt => tt == testTypeId);
            if (existingTestType == null || existingTestType == 0)
            {
                throw new ArgumentException($"Test type with id '{testTypeId}' does not exist.");
            }
        }
        var availablePanels = await _context.TestPanels.Select(x => x.TestPanelId).ToListAsync();
        foreach (int panelId in tests.PanelsIds)
        {
            var existingPanel = availablePanels.FirstOrDefault(tt => tt == panelId);
            if (existingPanel == null || existingPanel == 0)
            {
                throw new ArgumentException($"Panel with id '{panelId}' does not exist.");
            }
        }
        var batch = new TestBatch
        {
            PatientId = patientId,
            ReceptionistId = receptionistId,
            TestOrders = new List<TestOrder>(),
            Cost = 0.0m
        };

        Random rand = new Random();
        foreach (int testTypeId in tests.TestTypesIds)
        {
            List<Laboratory> labs = await _context.Laboratories.Where(x => x.TestTypes.Select(ttype => ttype.TestTypeId).Contains(testTypeId)).ToListAsync();
            if (labs.Count == 0)
            {
                throw new Exception($"No labs which can perform panel ID {testTypeId}");
            }
            decimal cost = await _context.TestTypes.Where(x => x.TestTypeId == testTypeId).Select(x => x.Cost).FirstOrDefaultAsync();

            var testOrder = new TestOrder
            {
                TestTypeId = testTypeId,
                TestBatchId = batch.TestBatchId,
                LaboratoryId = labs[rand.Next(0, labs.Count)].LaboratoryId
            };
            batch.TestOrders.Add(testOrder);
            batch.Cost += cost;
        }
        foreach (int panelId in tests.PanelsIds)
        {
            List<int> panelContents = await _context.TestPanels.Where(panel => panel.TestPanelId == panelId).SelectMany(x => x.TestTypes.Select(ttype => ttype.TestTypeId)).ToListAsync();
            List<Laboratory> labs = await _context.Laboratories
                .Include(lab => lab.TestTypes).ToListAsync();
            List<Laboratory> suitableLabs = labs.Where(lab => panelContents.All(id => lab.TestTypes.Select(x => x.TestTypeId).Contains(id))).ToList();
            if (labs.Count == 0)
            {
                throw new Exception($"No labs which can perform panel ID {panelId}");
            }
            decimal cost = await _context.TestPanels.Where(x => x.TestPanelId == panelId).Select(x => x.Cost).FirstOrDefaultAsync();
            foreach (int testTypeId in panelContents)
            {

                var testOrder = new TestOrder
                {
                    TestTypeId = testTypeId,
                    TestBatchId = batch.TestBatchId,
                    LaboratoryId = labs[rand.Next(0, labs.Count)].LaboratoryId,
                    TestPanelId = panelId
                };
                batch.TestOrders.Add(testOrder);
            }
            batch.Cost += cost;
        }
        _context.TestBatches.Add(batch);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Couldn't add order. Code 432");
        }
    }
}