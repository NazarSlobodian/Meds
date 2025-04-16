using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
        List<TechnicianPanelInfo> panels = await _context.TestPanels.Include(x => x.TestTypes).Select(xx => new TechnicianPanelInfo(xx)).ToListAsync();
        await _activityLoggerService.Log("Requesting test panels", null, null, "success");
        return panels;
    }
    public async Task<List<AdminTestTypeInfo>> GetTestTypesForAdminView()
    {
        List<AdminTestTypeInfo> tbs = await _context.TestTypes
            .Select(tt => new AdminTestTypeInfo
            {
                TestTypeId = tt.TestTypeId,
                Name = tt.Name,
                Cost = tt.Cost,
                MeasurementsUnit = tt.MeasurementsUnit
            })
            .ToListAsync();
        await _activityLoggerService.Log("Requesting test types", null, null, "success");
        return tbs;
    }
    public async Task AddTestType(AdminTestTypeNew info)
    {
        await _context.TestTypes.AddAsync(new TestType { Cost = info.Cost, MeasurementsUnit = info.MeasurementsUnit, Name = info.Name });
        await _context.SaveChangesAsync();
        await _activityLoggerService.Log("Adding test type", null, null, "success");
    }
    public async Task UpdateTestType(AdminTestTypeInfo test)
    {
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
                MaxResValue= tnv.MaxResValue,
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
}