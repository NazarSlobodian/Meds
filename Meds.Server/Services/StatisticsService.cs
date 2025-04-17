using Humanizer;
using Meds.Server.Models.DbModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QuestPDF.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class StatisticsService
{
    private readonly Wv1Context _context;
    private readonly ActivityLoggerService _activityLoggerService;
    public StatisticsService(Wv1Context context, ActivityLoggerService activityLoggerService)
    {
        _context = context;
        _activityLoggerService = activityLoggerService;
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
        await _activityLoggerService.Log("Requesting revenue statistics", null, null, "success");
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
        await _activityLoggerService.Log("Requesting age distribution statistics", null, null, "success");
        return ageDistribution;
    }
    public async Task<List<NamedList<YearlyOrders>>> GetTestOrdersStats()
    {

        var allTestTypes = await _context.TestTypes
            .Select(tt => new { tt.TestTypeId, tt.Name, tt.Cost })
            .ToListAsync();

        var statsSeparate = await _context.TestOrders
            //.Where(to => to.TestPanelId == null)
            .Select(to => new
            {
                Year = to.TestBatch.DateOfCreation.Year,
                Month = to.TestBatch.DateOfCreation.Month,
                TestTypeId = to.TestTypeId,
                Cost = to.TestType.Cost
            })
            .ToListAsync();

        List<YearlyOrders> groupedSeparate = statsSeparate
            .GroupBy(x => x.Year)
            .Select(g => new YearlyOrders
            {
                Year = g.Key,
                Stats = g.GroupBy(x => x.Month)
                    .Select(mg => new MonthlyOrders
                    {
                        Month = mg.Key,
                        Stats = allTestTypes.Select(tt => new TestOrdersData
                        {
                            Name = tt.Name,
                            CostPerOne = tt.Cost,
                            Count = mg.Count(x => x.TestTypeId == tt.TestTypeId)
                        }).ToList()
                    }).OrderBy(m => m.Month).ToList()
            }).OrderBy(y => y.Year).ToList();

        var allTestPanels = await _context.TestPanels
            .Select(tp => new { tp.TestPanelId, tp.Name, tp.Cost, AmountOfTestsInside = tp.TestTypes.Count() })
            .ToListAsync();

        var statsPanels = await _context.TestOrders
           .Where(to => to.TestPanelId != null)
           .Select(to => new
           {
               Year = to.TestBatch.DateOfCreation.Year,
               Month = to.TestBatch.DateOfCreation.Month,
               TestPanelId = to.TestPanelId,
               Cost = to.TestType.Cost,

           })
           .ToListAsync();

        List<YearlyOrders> groupedPanels = statsPanels
            .GroupBy(x => x.Year)
            .Select(g => new YearlyOrders
            {
                Year = g.Key,
                Stats = g.GroupBy(x => x.Month)
                    .Select(mg => new MonthlyOrders
                    {
                        Month = mg.Key,
                        Stats = allTestPanels.Select(tt => new TestOrdersData
                        {
                            Name = tt.Name,
                            CostPerOne = tt.Cost,
                            Count = mg.Count(x => x.TestPanelId == tt.TestPanelId) / tt.AmountOfTestsInside
                        }).ToList()
                    }).OrderBy(m => m.Month).ToList()
            }).OrderBy(y => y.Year).ToList();

        List<NamedList<YearlyOrders>> stats = new List<NamedList<YearlyOrders>>() {
            new NamedList<YearlyOrders> { Name = "Tests", List = groupedSeparate },
            new NamedList<YearlyOrders> { Name = "Panels", List = groupedPanels } };
        await _activityLoggerService.Log("Requesting test orders statistics", null, null, "success");
        return stats;

    }
    public async Task<ListWithTotalCount<ActivityLogAdmin>> GetActivityLogs(DateTime begin, DateTime end, int page, int pageSize)
    {

        var actLog = _context.ActivityLogs.AsQueryable();
        actLog = actLog
            .Where(l => l.DateTime > begin && l.DateTime < end);

        List<ActivityLog> list = await actLog
            .OrderBy(l => l.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        if (list.Count == 0)
        {
            throw new Exception("No records found");
        }
        int count = await actLog.CountAsync();
        await _activityLoggerService.Log("Activity logs requested", null, null, "success");
        return new ListWithTotalCount<ActivityLogAdmin>
            (list.Select(l => new ActivityLogAdmin
            {
                Action = l.Action,
                DateTime = l.DateTime,
                Actor = l.Actor,
                Description = l.Description,
                Status = l.Status
            }
        ).ToList(), count);
    }
}