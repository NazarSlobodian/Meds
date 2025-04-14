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
        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetYearlyRevenueStats()
        {
            List<YearlyRevenueStat> list;
            try
            {
                list = await _statisticsService.GetYearlyRevenueStats();
            }
            return Ok(list);
        }
    }
}