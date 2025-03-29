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
    public async Task<List<AdminTestTypeInfo>> GetTestTypesForAdminView()
    {
        List<AdminTestTypeInfo> tbs = await _context.TestTypes
            .Select(tt => new AdminTestTypeInfo
            {
                TestTypeId = tt.TestTypeId,
                Name = tt.Name,
                Cost = tt.Cost,
                DaysTillOverdue = tt.DaysTillOverdue,
                MeasurementsUnit = tt.MeasurementsUnit
            })
            .ToListAsync();
        return tbs;
    }
    public async Task AddTestType(AdminTestTypeNew info)
    {
        await _context.TestTypes.AddAsync(new TestType { Cost = info.Cost, DaysTillOverdue = info.DaysTillOverdue, MeasurementsUnit = info.MeasurementsUnit, Name = info.Name });
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
        tt.DaysTillOverdue = test.DaysTillOverdue;
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
        if (tnv.Gender != "m" && tnv.Gender != "f")
        {
            throw new Exception("Invalid gender");
        }
        if (tnv.MaxResValue < tnv.MinResValue)
        {
            throw new Exception("Invalid res gap");
        }
        if (tnv.MaxAge < tnv.MinAge)
        {
            throw new Exception("Invalid res gap");
        }
        TestNormalValue? otherNV = await _context.TestNormalValues.Where(t => t.Gender == tnv.Gender && t.TestTypeId == tnv.TestTypeId && (
        (tnv.MinAge <= t.MaxAge && tnv.MinAge >= t.MinAge) || (tnv.MaxAge <= t.MaxAge && tnv.MaxAge >= t.MinAge) ||
        (tnv.MinAge <= t.MinAge && tnv.MaxAge >= t.MaxAge))).FirstOrDefaultAsync();
        if (otherNV != null)
        {
            throw new Exception("Overlap");
        }
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
        if (tnv == null)
        {
            throw new Exception("Test type not found");
        }
        if (info.Gender != "m" && info.Gender != "f")
        {
            throw new Exception("Invalid gender");
        }
        if (info.MaxResValue < info.MinResValue)
        {
            throw new Exception("Invalid res gap");
        }
        if (info.MaxAge < info.MinAge)
        {
            throw new Exception("Invalid age gap");
        }
        TestNormalValue? otherNV = await _context.TestNormalValues.Where(t=> t.Gender == info.Gender && t.TestTypeId == tnv.TestTypeId && t.TestNormalValueId != info.TestNormalValueId && (
        (info.MinAge <= t.MaxAge && info.MinAge >= t.MinAge) || (info.MaxAge <= t.MaxAge && info.MaxAge >= t.MinAge) ||
        (info.MinAge <= t.MinAge && info.MaxAge >= t.MaxAge))).FirstOrDefaultAsync();
        if (otherNV != null)
        {
            throw new Exception("Overlap");
        }
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