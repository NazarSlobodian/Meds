using Meds.Server.Models.DbModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace Meds.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestTypesController : ControllerBase
    {

        private readonly TestTypesService _testTypesService;
        public TestTypesController(TestTypesService testTypesService)
        {
            _testTypesService = testTypesService;
        }

        [HttpGet("receptionist")]
        [Authorize(Policy = "Receptionist")]
        public async Task<IActionResult> GetTestTypes()
        {
            List<TechnicianTestTypeInfo> testTypes = await _testTypesService.GetTestTypesForTechView();
            List<TechnicianPanelInfo> panel = await _testTypesService.GetPanelsForTechView();
            return Ok(new { testTypes = testTypes, panelInfo = panel });
        }
        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetTestTypesGeneralInfo([FromQuery] int page, [FromQuery] int pageSize)
        {
            ListWithTotalCount<AdminTestTypeInfo> testTypes = await _testTypesService.GetTestTypesForAdminView(page, pageSize);
            return Ok(testTypes);
        }
        [HttpGet("admin/panels")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetTestPanelsGeneralInfo([FromQuery] int page, [FromQuery] int pageSize)
        {
            ListWithTotalCount<AdminTestPanelInfo> testTypes = await _testTypesService.GetTestPanelsForAdminView(page, pageSize);
            return Ok(testTypes);
        }
        [HttpPost("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddTestType([FromBody] AdminTestTypeNew info)
        {
            try
            {
                await _testTypesService.AddTestType(info);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok(new { message = "added" });
        }
        [HttpPut("admin/{testTypeId}/toggle")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ToggleTestType(int testTypeId)
        {
            try
            {
                await _testTypesService.ToggleTestTypeIsActive(testTypeId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpDelete("admin/{testTypeId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteTestType(int testTypeId)
        {
            try
            {
                await _testTypesService.DeleteTestType(testTypeId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpPost("admin/panels")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddTestPanel([FromBody] AdminTestPanelNew info)
        {
            try
            {
                await _testTypesService.AddTestPanel(info);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpPut("admin/panels/{panelId}/toggle")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ToggleTestPanel(int panelId)
        {
            try
            {
                await _testTypesService.TogglePanelIsActive(panelId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpDelete("admin/panels/{panelId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteTestPanel(int panelId)
        {
            try
            {
                await _testTypesService.DeleteTestPanel(panelId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpPut("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateTestTypeGeneralInfo([FromBody] AdminTestTypeInfo info)
        {
            try
            {
                await _testTypesService.UpdateTestType(info);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok(new { message = "updated" });
        }
        [HttpPut("admin/panels")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateTestPanelGeneralInfo([FromBody] AdminTestPanelInfo info)
        {
            try
            {
                await _testTypesService.UpdateTestPanel(info);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok(new { message = "updated" });
        }
        [HttpGet("admin/panels/{panelId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetPanelContents(int panelId)
        {
            List<TestAvailability> tests;
            try
            {
                tests = await _testTypesService.GetPanelContents(panelId);
            }
            catch (Exception ex)
            {
                return BadRequest("Couldn't get a list of available test types");
            }
            return Ok(tests);
        }
        [HttpGet("admin/normals/{testTypeID}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetTestNormalValeusGeneralInfo(int testTypeID)
        {
            List<TestNormalValueDTO> testNormalValues = await _testTypesService.GetTestNormalValues(testTypeID);
            return Ok(testNormalValues);
        }
        [HttpPost("admin/normals")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddTestNormalValue(AdminTestNormalValueNew tnv)
        {
            try
            {
                await _testTypesService.AddTestNormalValue(tnv);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
            return Ok(new { message = "added" });
        }
        [HttpPut("admin/normals")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateTestNormalValue(AdminTestNormalValue info)
        {
            try
            {
                await _testTypesService.UpdateTestNormalValue(info);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
            return Ok(new { message = "updated" });
        }
        [HttpDelete("admin/normals/{testNormalValueID}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteTestNormalValue(int testNormalValueID)
        {
            try
            {
                await _testTypesService.DeleteTestNormalValue(testNormalValueID);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok(new { message = "deleted" });
        }
        [HttpPut("lab-admin/availableTests")]
        [Authorize(Policy = "LabAdmin")]
        public async Task<IActionResult> UpdateAvailableTests([FromBody] List<TestAvailability> testTypeStates)
        {
            Claim? labAdminIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (labAdminIdClaim == null)
            {
                return Unauthorized(new { message = "No id in claims" });
            }
            int labAdminId = int.Parse(labAdminIdClaim.Value);
            try
            {
                await _testTypesService.UpdateAvailableTestTypes(testTypeStates, labAdminId);
            }
            catch (Exception ex)
            {
                return BadRequest("Couldn't update list of available test types");
            }
            return Ok();
        }

        [HttpPut("admin/panels/{panelId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdatePanelContents([FromBody] List<TestAvailability> panelContents, int panelId)
        {
            try
            {
                await _testTypesService.UpdatePanelContents(panelContents, panelId);
            }
            catch (Exception ex)
            {
                return BadRequest("Couldn't update list of available test types");
            }
            return Ok();
        }
        [HttpGet("lab-admin/availableTests")]
        [Authorize(Policy = "LabAdmin")]
        public async Task<IActionResult> GetAvailableTests()
        {
            Claim? labAdminIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (labAdminIdClaim == null)
            {
                return Unauthorized(new { message = "No id in claims" });
            }
            int labAdminId = int.Parse(labAdminIdClaim.Value);
            List<TestAvailability> tests;
            try
            {
                tests = await _testTypesService.GetAvailableTestTypes(labAdminId);
            }
            catch (Exception ex)
            {
                return BadRequest("Couldn't get a list of available test types");
            }
            return Ok(tests);
        }
    }
}