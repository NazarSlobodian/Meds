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
                Adress = tb.Receptionist.CollectionPoint.Address,
                Year = tb.DateOfCreation.Year,
                Month = tb.DateOfCreation.Month,
                tb.Cost
            })
            .GroupBy(x => x.Year)
            .Select(g => new YearlyCollectionPointRevenueStat
            {
                Year = g.Key,
                Stats = g.GroupBy(x => x.Month)
                    .Select(mg => new MonthlyCollectionPointRevenueStat
                    {
                        Month = mg.Key,
                        Stats = mg.GroupBy(x => x.Adress)
                        .Select(gg => new CollectionPointRevenueStat
                        {
                            Address = gg.Key,
                            Revenue = gg.Sum(x => x.Cost)
                        })
                        .ToList()
                    })
                    .OrderBy(m => m.Month)
                    .ToList()
            })
            .OrderBy(x => x.Year)
            .ToListAsync();
        return stats;
    }
}