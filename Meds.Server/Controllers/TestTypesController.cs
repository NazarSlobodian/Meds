using Meds.Server.Models.DBModels;
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

        [HttpGet("technician")]
        [Authorize(Policy = "Technician")]
        public async Task<IActionResult> GetTestTypes()
        {
            List<TechnicianTestTypeInfo> testTypes = await _testTypesService.GetTestTypesForTechView();
            return Ok(testTypes);
        }
        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetTestTypesGeneralInfo()
        {
            List<AdminTestTypeInfo> testTypes = await _testTypesService.GetTestTypesForAdminView();
            return Ok(testTypes);
        }
        [HttpPost("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddTestTypeGeneralInfo([FromBody] AdminTestTypeNew info)
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
                return BadRequest(new { message = ex.Message });
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
                return BadRequest(new { message = ex.Message });
            }
            return Ok(new { message = "updated"});
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
    }
}