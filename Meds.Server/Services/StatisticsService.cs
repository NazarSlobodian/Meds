using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;

public class StatisticsService
{
    private readonly Wv1Context _context;
    public StatisticsService(Wv1Context context)
    {
        _context = context;
    }

    public async Task<List<YearlyCollectionPointRevenueStat>> GetYearlyRevenueStats()
    {
        List<YearlyCollectionPointRevenueStat> stats = await _context.TestBatches
            .Select(tb => new
            {
                tb.Receptionist.CollectionPoint.Address,
                Year = tb.DateOfCreation.Year,
                Month = tb.DateOfCreation.Month,
                tb.Cost
            })
            .GroupBy(x => new { x.Address, x.Year })
            .Select(g => new YearlyCollectionPointRevenueStat
            {
                LabAddress = g.Key.Address,
                Year = g.Key.Year,
                Stats = g.GroupBy(x => x.Month)
                    .Select(mg => new MonthlyCollectionPointRevenueStat
                    {
                        Month = mg.Key,
                        Revenue = mg.Sum(x => x.Cost)
                    })
                    .OrderBy(m => m.Month)
                    .ToList()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.LabAddress)
            .ToListAsync();
        return stats;
    }
}