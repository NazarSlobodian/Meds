using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Security.Claims;

namespace Meds.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {

        private readonly StatisticsService _statisticsService;
        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpGet("yearlyRevenue")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetYearlyRevenueStats()
        {
            List<YearlyCollectionPointRevenueStat> list;
            try
            {
                list = await _statisticsService.GetYearlyRevenueStats();
            }
            catch
            {
                return BadRequest(new { message = "Couldn't get statistics" });
            }
            return Ok(list);
        }
    }
}