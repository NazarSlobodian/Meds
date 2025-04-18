using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Net.Mime.MediaTypeNames;

public class TestTypesService
{
    private readonly Wv1Context _context;
    private readonly ActivityLoggerService _activityLoggerService;
    public TestTypesService(Wv1Context context, ActivityLoggerService activityLoggerService)
    {
        _context = context;
        _activityLoggerService = activityLoggerService;
    }

    public async Task<List<TechnicianTestTypeInfo>> GetTestTypesForTechView()
    {
        List<TechnicianTestTypeInfo> tbs = await _context.TestTypes
            .Where(tt => tt.IsActive == 1)
            .Select(tt => new TechnicianTestTypeInfo
            {
                TestTypeId = tt.TestTypeId,
                Name = tt.Name,
                Cost = tt.Cost
            })
            .ToListAsync();
        await _activityLoggerService.Log("Requesting test types", null, null, "success");
        return tbs;
    }
    public async Task<List<TechnicianPanelInfo>> GetPanelsForTechView()
    {
        List<TechnicianPanelInfo> panels = await _context.TestPanels
            .Include(x => x.TestTypes)
            .Where(tp => tp.IsActive == 1)
            .Where(tp => tp.TestTypes.All(tt => tt.IsActive == 1))
            .Select(xx => new TechnicianPanelInfo(xx)).ToListAsync();
        await _activityLoggerService.Log("Requesting test panels", null, null, "success");
        return panels;
    }
    public async Task<ListWithTotalCount<AdminTestTypeInfo>> GetTestTypesForAdminView(int page, int pageSize)
    {
        var query = _context.TestTypes.OrderBy(t => t.TestTypeId).AsQueryable();
        List<AdminTestTypeInfo> tbs = await query
            .Select(tt => new AdminTestTypeInfo
            {
                TestTypeId = tt.TestTypeId,
                Name = tt.Name,
                Cost = tt.Cost,
                MeasurementsUnit = tt.MeasurementsUnit,
                IsActive = tt.IsActive == 1 ? true : false
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        ListWithTotalCount<AdminTestTypeInfo> tbbs = new ListWithTotalCount<AdminTestTypeInfo> { List = tbs, TotalCount = await query.CountAsync() };
        await _activityLoggerService.Log("Requesting test types", null, null, "success");
        return tbbs;
    }
    public async Task<ListWithTotalCount<AdminTestPanelInfo>> GetTestPanelsForAdminView(int page, int pageSize)
    {
        var query = _context.TestPanels.OrderBy(t => t.TestPanelId).AsQueryable();
        List<AdminTestPanelInfo> tbs = await query
            .Select(tp => new AdminTestPanelInfo
            {
                TestPanelId = tp.TestPanelId,
                Name = tp.Name,
                Cost = tp.Cost,
                IsActive = tp.IsActive == 1 ? true : false
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        ListWithTotalCount<AdminTestPanelInfo> tbbs = new ListWithTotalCount<AdminTestPanelInfo> { List = tbs, TotalCount = await query.CountAsync() };
        await _activityLoggerService.Log("Requesting test panels", null, null, "success");
        return tbbs;
    }

    public async Task AddTestType(AdminTestTypeNew info)
    {
        if (info.MeasurementsUnit.Trim() == "" || info.Name.Trim() == "" || info.Cost < 0.0m)
        {
            throw new Exception("Invalid data");
        }
        try
        {
            await _context.TestTypes.AddAsync(new TestType { Cost = info.Cost, MeasurementsUnit = info.MeasurementsUnit, Name = info.Name });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _activityLoggerService.Log("Adding test type", null, null, "fail");
            throw ex;
        }
        await _activityLoggerService.Log("Adding test type", null, null, "success");
    }
    public async Task AddTestPanel(AdminTestPanelNew info)
    {
        if (info.Name.Trim() == "" || info.Cost < 0.0m)
        {
            throw new Exception("Invalid data");
        }
        try
        {
            await _context.TestPanels.AddAsync(new TestPanel { Cost = info.Cost, Name = info.Name });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _activityLoggerService.Log("Adding test panel", null, null, "fail");
            throw ex;
        }
        await _activityLoggerService.Log("Adding test panel", null, null, "success");
    }
    public async Task UpdateTestType(AdminTestTypeInfo test)
    {
        if (test.MeasurementsUnit.Trim() == "" || test.Name.Trim() == "" || test.Cost < 0.0m)
        {
            throw new Exception("Invalid data");
        }
        TestType? tt = await _context.TestTypes.FirstOrDefaultAsync(t => t.TestTypeId == test.TestTypeId);
        if (tt == null)
        {
            await _activityLoggerService.Log("Updating test type", null, null, "fail");
            throw new Exception("Test type not found");
        }
        tt.MeasurementsUnit = test.MeasurementsUnit;
        tt.Cost = test.Cost;
        tt.Name = test.Name;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Updating test type", null, null, "success");
    }
    public async Task ToggleTestTypeIsActive(int testTypeId)
    {
        TestType? tt = await _context.TestTypes.FirstOrDefaultAsync(t => t.TestTypeId == testTypeId);
        if (tt == null)
        {
            await _activityLoggerService.Log("Toggling test type active", null, null, "fail");
            throw new Exception("Test type not found");
        }
        if (tt.IsActive == 1)
            tt.IsActive = 0;
        else
            tt.IsActive = 1;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Toggling test type active", null, null, "success");
    }
    public async Task DeleteTestType(int testTypeId)
    {
        TestType? tt = await _context.TestTypes.FirstOrDefaultAsync(t => t.TestTypeId == testTypeId);
        if (tt == null)
        {
            await _activityLoggerService.Log("Deleting test type", null, null, "fail");
            throw new Exception("Test type not found");
        }
        try
        {
            _context.TestTypes.Remove(tt);
            await _context.SaveChangesAsync();
            await _activityLoggerService.Log("Deleting test type", null, null, "success");
        }
        catch (Exception ex)
        {
            throw new Exception("Test type cannot be deleted as it is already used in orders. Try disabling instead");
        }
    }
    public async Task UpdateTestPanel(AdminTestPanelInfo test)
    {
        if (test.Name.Trim() == "" || test.Cost < 0.0m)
        {
            throw new Exception("Invalid data");
        }
        TestPanel? tt = await _context.TestPanels.FirstOrDefaultAsync(t => t.TestPanelId == test.TestPanelId);
        if (tt == null)
        {
            await _activityLoggerService.Log("Updating test panel", null, null, "fail");
            throw new Exception("Test type not found");
        }
        tt.Cost = test.Cost;
        tt.Name = test.Name;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Updating test panel", null, null, "success");
    }
    public async Task TogglePanelIsActive(int panelId)
    {
        TestPanel? tp = await _context.TestPanels.FirstOrDefaultAsync(t => t.TestPanelId == panelId);
        if (tp == null)
        {
            await _activityLoggerService.Log("Toggling test panel active", null, null, "fail");
            throw new Exception("Test panel not found");
        }
        if (tp.IsActive == 1)
            tp.IsActive = 0;
        else
            tp.IsActive = 1;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Toggling test panel active", null, null, "success");
    }
    public async Task DeleteTestPanel(int testPanelId)
    {
        TestPanel? tp = await _context.TestPanels.FirstOrDefaultAsync(t => t.TestPanelId == testPanelId);
        if (tp == null)
        {
            await _activityLoggerService.Log("Deleting test panel", null, null, "fail");
            throw new Exception("Test panel not found");
        }
        try
        {
            _context.TestPanels.Remove(tp);
            await _context.SaveChangesAsync();
            await _activityLoggerService.Log("Deleting test panel", null, null, "success");
        }
        catch (Exception ex)
        {
            throw new Exception("Test panel cannot be deleted as it is already used in orders. Try disabling instead");
        }
    }
    public async Task<List<TestNormalValueDTO>> GetTestNormalValues(int testTypeID)
    {
        List<TestNormalValueDTO> tnv = await _context.TestNormalValues
            .Select(tnv => new TestNormalValueDTO
            {
                TestNormalValueId = tnv.TestNormalValueId,
                TestTypeId = tnv.TestTypeId,
                Gender = tnv.Gender,
                MinAge = tnv.MinAge,
                MaxAge = tnv.MaxAge,
                MinResValue = tnv.MinResValue,
                MaxResValue = tnv.MaxResValue,
            })
            .Where(tnv => tnv.TestTypeId == testTypeID)
            .ToListAsync();
        await _activityLoggerService.Log("Requesting test type normal values", null, null, "success");
        return tnv;
    }
    public async Task AddTestNormalValue(AdminTestNormalValueNew tnv)
    {
        await _context.TestNormalValues.AddAsync(
            new TestNormalValue()
            {
                TestTypeId = tnv.TestTypeId,
                Gender = tnv.Gender,
                MinAge = tnv.MinAge,
                MaxAge = tnv.MaxAge,
                MinResValue = tnv.MinResValue,
                MaxResValue = tnv.MaxResValue,
            }
            );
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Adding test type normal value", null, null, "success");
    }
    public async Task UpdateTestNormalValue(AdminTestNormalValue info)
    {
        TestNormalValue? tnv = await _context.TestNormalValues.FirstOrDefaultAsync(t => t.TestNormalValueId == info.TestNormalValueId);
        tnv.MinAge = info.MinAge;
        tnv.MaxAge = info.MaxAge;
        tnv.Gender = info.Gender;
        tnv.MinResValue = info.MinResValue;
        tnv.MaxResValue = info.MaxResValue;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Updating test type normal value", null, null, "success");
    }
    public async Task DeleteTestNormalValue(int testNormalValueID)
    {
        TestNormalValue? tn = await _context.TestNormalValues.FirstOrDefaultAsync(tnv => tnv.TestNormalValueId == testNormalValueID);
        if (tn == null)
        {
            await _activityLoggerService.Log("Deleting test type normal value", null, null, "fail");
            return;
        }
        _context.TestNormalValues.Remove(tn);
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Deleting test type normal value", null, null, "success");
    }
    public async Task<List<TestAvailability>> GetPanelContents(int panelId)
    {
        TestPanel? tp = await _context.TestPanels.Include(tp => tp.TestTypes).Where(tp => tp.TestPanelId == panelId).FirstOrDefaultAsync();
        if (tp == null)
        {
            await _activityLoggerService.Log("Panel contents request", null, null, "fail");
            throw new Exception("Panel doesn't exits");
        }
        List<TestAvailability> tests = await _context.TestTypes
            .Select(tt => new TestAvailability
            {
                TestTypeId = tt.TestTypeId,
                Name = tt.Name,
                IsAvailable = tp.TestTypes.Contains(tt)
            })
            .OrderBy(tt => tt.Name)
            .ToListAsync();
        await _activityLoggerService.Log("Panel contents request", null, null, "success");
        return tests;
    }
    public async Task<List<TestAvailability>> GetAvailableTestTypes(int labAdminId)
    {
        int labId = await _context.Laboratories.Where(lab => lab.LabWorkers.Any(worker => worker.LabWorkerId == labAdminId)).Select(lab => lab.LaboratoryId).FirstOrDefaultAsync();
        if (labId == 0)
        {
            await _activityLoggerService.Log("Lab test availability request", null, null, "fail");
            throw new Exception("Admin not in lab");
        }
        List<TestAvailability> tests = await _context.TestTypes
            .Select(tt => new TestAvailability
            {
                TestTypeId = tt.TestTypeId,
                Name = tt.Name,
                IsAvailable = tt.Laboratories.Any(lab => lab.LaboratoryId == labId)
            })
            .OrderBy(tt => tt.Name)
            .ToListAsync();
        await _activityLoggerService.Log("Lab test availability request", null, null, "success");
        return tests;
    }
    public async Task UpdateAvailableTestTypes(List<TestAvailability> tests, int labAdminId)
    {
        int labId = await _context.Laboratories.Where(lab => lab.LabWorkers.Any(worker => worker.LabWorkerId == labAdminId)).Select(lab => lab.LaboratoryId).FirstOrDefaultAsync();
        if (labId == 0)
        {
            await _activityLoggerService.Log("Lab test availability change", null, null, "fail");
            throw new Exception("Admin not in lab");
        }
        List<int> ids = tests.Where(t => t.IsAvailable).Select(x => x.TestTypeId).ToList();


        List<TestType> available = await _context.TestTypes
            .Where(tt => ids
                .Contains(tt.TestTypeId))
            .ToListAsync();


        Laboratory? lab = await _context.Laboratories.Where(lab => lab.LaboratoryId == labId).Include(lab => lab.TestTypes).FirstOrDefaultAsync();
        if (lab == null)
        {
            await _activityLoggerService.Log("Lab test availability change", null, null, "fail");
            throw new Exception("Lab not found");
        }
        lab.TestTypes = available;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Lab test availability change", null, null, "success");
    }
    public async Task UpdatePanelContents(List<TestAvailability> tests, int panelId)
    {
        List<int> ids = tests.Where(t => t.IsAvailable).Select(x => x.TestTypeId).ToList();


        List<TestType> available = await _context.TestTypes
            .Where(tt => ids
                .Contains(tt.TestTypeId))
            .ToListAsync();


        TestPanel? panel = await _context.TestPanels.Where(panel => panel.TestPanelId == panelId).Include(lab => lab.TestTypes).FirstOrDefaultAsync();
        if (panel == null)
        {
            await _activityLoggerService.Log("Panel contents change", null, null, "fail");
            throw new Exception("Lab not found");
        }
        panel.TestTypes = available;
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Panel contents change", null, null, "success");
    }
}