using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
    public async Task<List<AgeCount>> GetAgeDistributionStats()
    {
        int[] ageGroups = new[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100 };

        DateTime now = DateTime.Now;
        var patients = await _context.Patients
            .Select(p => new
            {
                Gender = p.Gender,
                Age = now.Year - p.DateOfBirth.Year - ((now.DayOfYear < p.DateOfBirth.DayOfYear) ? 1 : 0)
            })
            .ToListAsync();
        List<AgeCount> ageDistribution = patients
            .GroupBy(p => new
            {
                p.Gender,
                AgeGroup = ageGroups
                .Select((minAge, i) => new { Min = minAge, Max = (i + 1 < ageGroups.Length ? ageGroups[i + 1] - 1 : int.MaxValue) })
                .First(group => p.Age >= group.Min && p.Age <= group.Max)
            })
            .Select(g => new AgeCount
            {
                Gender = g.Key.Gender,
                AgeGroup = $"{g.Key.AgeGroup.Min}-{(g.Key.AgeGroup.Max == int.MaxValue ? "100+" : g.Key.AgeGroup.Max.ToString())}",
                Count = g.Count()
            })
            .OrderBy(x => x.Gender)
            .ThenBy(g =>
            {
                var start = g.AgeGroup.Contains("+")
                    ? int.Parse(g.AgeGroup.Replace("+", ""))
                    : int.Parse(g.AgeGroup.Split('-')[0]);
                return start;
            })
            .ToList();

        return ageDistribution;
    }
}