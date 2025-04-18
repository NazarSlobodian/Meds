using Humanizer;
using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Meds.Server.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Cryptography.Xml;

public class PatientsService
{
    private readonly Wv1Context _context;
    private readonly MailService _mailService;
    private readonly ActivityLoggerService _activityLoggerService;
    public PatientsService(Wv1Context context, MailService mailService, ActivityLoggerService activityLoggerService)
    {
        _context = context;
        _mailService = mailService;
        _activityLoggerService = activityLoggerService;
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
            {
                await _activityLoggerService.Log("Patients request", null, null, "fail");
                return new ListWithTotalCount<PatientDTO>();
            }
        }
        List<Patient> tbs = await query.OrderBy(p => p.FullName).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        int count = await query.CountAsync();
        await _activityLoggerService.Log("Patients request", null, null, "success");
        return new ListWithTotalCount<PatientDTO>(tbs.Select(tb => new PatientDTO(tb)).ToList(), count);
    }
    public async Task<List<TestBatchDTO>> GetPatientBatchesAsync(int patientId)
    {
        List<TestBatch> tbs = await _context.TestBatches.Where(tb => tb.PatientId == patientId).OrderByDescending(tb=> tb.DateOfCreation).ToListAsync();
        await _activityLoggerService.Log("Batches request", null, null, "success");
        return tbs.Select(tb => new TestBatchDTO(tb)).ToList();
    }
    public async Task<ListWithTotalCount<TestBatchLabWorkerDTO>> GetLabWorkerBatchesAsync(int labWorkerId, int page, int pageSize)
    {
        int labId = await _context.Laboratories.Where(lab => lab.LabWorkers.Any(worker => worker.LabWorkerId == labWorkerId)).Select(lab => lab.LaboratoryId).FirstOrDefaultAsync();
        if (labId == 0)
        {
            await _activityLoggerService.Log("Batches request", null, null, "fail");
            throw new Exception("Worker not in lab");
        }
        var tbs = _context.TestBatches.AsQueryable();
        tbs = tbs.Include(tb => tb.TestOrders.Where(to => to.LaboratoryId == labId))
            .Where(tb => tb.TestOrders
                .Any(to => to.LaboratoryId == labId && to.TestResult == null)
                && tb.BatchStatus != "done");
        List<TestBatch> list = await tbs.OrderBy(x => x.DateOfCreation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        int count = await tbs.CountAsync();
        await _activityLoggerService.Log("Batches request", null, null, "success");
        return new ListWithTotalCount<TestBatchLabWorkerDTO>(list.Select(tb => new TestBatchLabWorkerDTO(tb)).ToList(), count);
    }
    public async Task<List<TestOrderLabWorkerDTO>> GetTestOrdersLabWorkerAsync(int testBatchId, int labWorkerId)
    {
        int labId = await _context.Laboratories.Where(lab => lab.LabWorkers.Any(worker => worker.LabWorkerId == labWorkerId)).Select(lab => lab.LaboratoryId).FirstOrDefaultAsync();
        if (labId == 0)
        {
            await _activityLoggerService.Log("Orders request", null, null, "fail");
            throw new Exception("Worker not in lab");
        }
        TestBatch? tb = await _context.TestBatches
            .Include(tb => tb.TestOrders
                .Where(to => to.LaboratoryId == labId))
                .ThenInclude(to => to.TestResult)
            .Include(tb => tb.TestOrders)
                .ThenInclude(to => to.TestType)
                .ThenInclude(tt => tt.TestNormalValues)
            .Include(tb => tb.Patient)
            .Where(tb => tb.TestBatchId == testBatchId && tb.BatchStatus != "done")
            .FirstOrDefaultAsync();

        if (tb == null)
        {
            await _activityLoggerService.Log("Orders request", null, null, "fail");
            throw new Exception("Test batch not found");
        }
        if (tb.TestOrders == null || tb.TestOrders.Count == 0)
        {
            await _activityLoggerService.Log("Orders request", null, null, "success");
            throw new Exception("All results are already sent");
        }

        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - tb.Patient.DateOfBirth.Year;
        if (tb.Patient.DateOfBirth > today.AddYears(-age))
            age--;
        string sex = tb.Patient.Gender;
        List<TestOrderLabWorkerDTO> list = new List<TestOrderLabWorkerDTO>();
        foreach (TestOrder to in tb.TestOrders)
        {
            string normalValue = "";
            try
            {
                TestNormalValue? tnv = to.TestType.TestNormalValues.Where(tnv => tnv.Gender == sex && age >= tnv.MinAge && age <= tnv.MaxAge).First();
                normalValue = tnv.MinResValue + " - " + tnv.MaxResValue;
            }
            catch
            {
                normalValue = "N/A";
            }
            list.Add(new TestOrderLabWorkerDTO
            {
                TestOrderId = to.TestOrderId,
                Name = to.TestType.Name,
                Result = to.TestResult?.Result,
                Unit = to.TestType.MeasurementsUnit,
                NormalValues = normalValue
            });
        }
        await _activityLoggerService.Log("Orders request", null, null, "success");
        return list;
    }
    public async Task<(List<TestOrderLabWorkerDTO>, int)> GetTestOrdersLabWorkerOrderAsync(int orderId, int labWorkerId)
    {
        int batchId = await _context.TestOrders.Where(x => x.TestOrderId == orderId).Select(x => x.TestBatchId).FirstAsync();
        return (await GetTestOrdersLabWorkerAsync(batchId, labWorkerId), batchId);
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
        await _activityLoggerService.Log("Patient addition", null, null, "success");
        return p.PatientId;
    }
    public async Task<BatchResultsDTO> GetBatchResultsAsync(int batchId, int patientId)
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
        await _activityLoggerService.Log("Batch result request", null, null, "success");
        if (patientId != batch.PatientId)
            throw new Exception("Nope");
        return new BatchResultsDTO(collectionPoint, batch);
    }
    public async Task<BatchResultsDTO> GetBatchResultsBYPASSAsync(int batchId)
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
        await _activityLoggerService.Log("Batch result request", null, null, "success");
        return new BatchResultsDTO(collectionPoint, batch);
    }

    public async Task ValidateAndSubmitBatchesAsync(int patientId, int receptionistId, NewOrder tests)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null)
        {
            await _activityLoggerService.Log("Order placement", null, null, "fail");
            throw new ArgumentException("Patient not found.");
        }
        if (tests == null || (tests.TestTypesIds.Count == 0 && tests.PanelsIds.Count == 0))
        {
            await _activityLoggerService.Log("Order placement", null, null, "fail");
            throw new ArgumentException("Can't add an empty order.");
        }
        var availableTestTypes = await _context.TestTypes.Select(x => x.TestTypeId).ToListAsync();
        foreach (int testTypeId in tests.TestTypesIds)
        {
            var existingTestType = availableTestTypes.FirstOrDefault(tt => tt == testTypeId);
            if (existingTestType == null || existingTestType == 0)
            {
                await _activityLoggerService.Log("Order placement", null, null, "fail");
                throw new ArgumentException($"Test type with id '{testTypeId}' does not exist.");
            }
        }
        var availablePanels = await _context.TestPanels.Select(x => x.TestPanelId).ToListAsync();
        foreach (int panelId in tests.PanelsIds)
        {
            var existingPanel = availablePanels.FirstOrDefault(tt => tt == panelId);
            if (existingPanel == null || existingPanel == 0)
            {
                await _activityLoggerService.Log("Order placement", null, null, "fail");
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
                TestType tt = await _context.TestTypes.Where(t => t.TestTypeId == testTypeId).FirstAsync();
                await _activityLoggerService.Log("Lab assigment", null, null, "fail");
                throw new Exception($"No labs which can perform test ID {tt.Name}");
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
            if (suitableLabs.Count == 0)
            {
                await _activityLoggerService.Log("Lab assigment", null, null, "fail");
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
            await _activityLoggerService.Log("Order placement", null, null, "success");
        }
        catch (Exception ex)
        {
            await _activityLoggerService.Log("Order placement", null, null, "fail");
            throw new ApplicationException($"Couldn't add order. Code 432");
        }
    }
    public async Task ValidateAndSubmitResultsAsync(List<TestOrderLabWorkerDTO> results, int labWorkerId)
    {
        int labId = await _context.Laboratories.Where(lab => lab.LabWorkers.Any(worker => worker.LabWorkerId == labWorkerId)).Select(lab => lab.LaboratoryId).FirstOrDefaultAsync();
        if (results == null || results.Count == 0)
        {
            await _activityLoggerService.Log("Saving results", null, null, "fail");
            throw new Exception("No results submitted");
        }
        int id = results[0].TestOrderId;
        List<TestOrder> orders = null;
        try
        {
            orders = await _context.TestBatches
                .Where(tb => tb.TestOrders
                    .Any(to => to.TestOrderId == id))
                .Include(tb => tb.TestOrders)
                    .ThenInclude(to => to.TestResult)
                .SelectMany(tb => tb.TestOrders)
                .Where(to => to.LaboratoryId == labId).ToListAsync();
        }
        catch (Exception ex)
        {
            await _activityLoggerService.Log("Saving results", null, null, "fail");
            throw new Exception("Couldn't find batch");
        }
        if (orders.Count != results.Count)
        {
            await _activityLoggerService.Log("Saving results", null, null, "fail");
            throw new Exception("Orders/results amount mismatch");
        }
        foreach (TestOrder order in orders)
        {
            TestOrderLabWorkerDTO? result = results.Find(t => t.TestOrderId == order.TestOrderId);
            if (result == null)
            {
                await _activityLoggerService.Log("Saving results", null, null, "fail");
                throw new Exception("Orders/results id mismatch");
            }
            if (result.Result == null)
            {
                TestResult? res = _context.TestResults.Find(result.TestOrderId);
                if (res != null)
                {
                    _context.TestResults.Remove(res);
                }
            }
            else if (order.TestResult == null)
            {
                _context.TestResults.Add(new TestResult() { TestOrderId = order.TestOrderId, Result = result.Result.Value });
            }
            else
            {
                order.TestResult.Result = result.Result.Value;
            }
        }
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Saving results", null, null, "success");



        // After saving, check if the batch status has been updated by the MySQL trigger
        var batchId = await _context.TestOrders
            .Where(r => r.TestOrderId == results[0].TestOrderId)
            .Select(r => r.TestBatchId)
            .FirstOrDefaultAsync();

        var batch = await _context.TestBatches
            .Include(b => b.TestOrders)
            .ThenInclude(o => o.TestResult)
            .FirstOrDefaultAsync(b => b.TestBatchId == batchId);
        if (batch.BatchStatus == "done") // assuming your trigger sets it to "Completed"
        {
            BatchResultsDTO dto = await GetBatchResultsBYPASSAsync(batchId);
            // Call the PDF generation logic from PdfService
            await _mailService.SendResultsAndSave(dto);
        }
    }
}