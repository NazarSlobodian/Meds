using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Meds.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Security.Claims;

namespace Meds.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExportController : ControllerBase
    {

        private readonly ExportService _exportService;
        public ExportController(ExportService exportService)
        {
            _exportService = exportService;
        }
        [HttpGet("jsonTask")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> GetBatchesJson()
        {
            try
            {
                return File(await _exportService.GetBatchesJson(), "application/json", "jsonBatches.json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}