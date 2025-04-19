using Meds.Server.Services;
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
            catch (Exception ex)
            {
                return BadRequest(new { message = "Couldn't get statistics" });
            }
            return Ok(list);
        }
        [HttpGet("ageDistribution")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAgeDistributionsStats()
        {
            List<AgeCount> list;
            try
            {
                list = await _statisticsService.GetAgeDistributionStats();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Couldn't get statistics" });
            }
            return Ok(list);
        }
        [HttpGet("testOrdersNumbers")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetTestOrdersStats()
        {
            List<NamedList<YearlyOrders>> list;
            try
            {
                list = await _statisticsService.GetTestOrdersStats();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Couldn't get statistics" });
            }
            return Ok(list);
        }
        [HttpPost("activityLogs")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetActivityLogs([FromBody] ActivityLogRequest req)
        {
            try
            {
                return Ok (await _statisticsService.GetActivityLogs(req.Begin, req.End, req.Page, req.PageSize));
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}