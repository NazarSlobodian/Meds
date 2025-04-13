using Meds.Server.Models;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;

public class TestTypesService
{
    private readonly Wv1Context _context;
    public TestTypesService(Wv1Context context)
    {
        _context = context;
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
        return tbs;
    }
    public async Task<List<TechnicianPanelInfo>> GetPanelsForTechView()
    {
        List<TechnicianPanelInfo> panels = await _context.TestPanels.Include(x => x.TestTypes).Select(xx => new TechnicianPanelInfo(xx)).ToListAsync();
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
        return tbs;
    }
    public async Task AddTestType(AdminTestTypeNew info)
    {
        await _context.TestTypes.AddAsync(new TestType { Cost = info.Cost, MeasurementsUnit = info.MeasurementsUnit, Name = info.Name });
        await _context.SaveChangesAsync();
    }
    public async Task UpdateTestType(AdminTestTypeInfo test)
    {
        TestType? tt = await _context.TestTypes.FirstOrDefaultAsync(t => t.TestTypeId == test.TestTypeId);
        if (tt == null)
        {
            throw new Exception("Test type not found");
        }
        tt.MeasurementsUnit = test.MeasurementsUnit;
        tt.Cost = test.Cost;
        tt.Name = test.Name;
        await _context.SaveChangesAsync();
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
    }
    public async Task DeleteTestNormalValue(int testNormalValueID)
    {
        TestNormalValue? tn = await _context.TestNormalValues.FirstOrDefaultAsync(tnv => tnv.TestNormalValueId == testNormalValueID);
        if (tn == null)
            return;
        _context.TestNormalValues.Remove(tn);
        await _context.SaveChangesAsync();
    }
}