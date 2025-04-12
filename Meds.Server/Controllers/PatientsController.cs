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
    public class PatientsController : ControllerBase
    {

        private readonly PatientsService _patientsService;
        public PatientsController(PatientsService patientsService)
        {
            _patientsService = patientsService;
        }
        [HttpGet("batches")]
        [Authorize(Policy = "Patient")]
        public async Task<IActionResult> GetTestBatches()
        {
            Claim? patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (patientIdClaim == null)
            {
                return Unauthorized(new { message = "No id in claims" });
            }
            int patientId = int.Parse(patientIdClaim.Value);
            List<TestBatchDTO> testBatches = await _patientsService.GetPatientBatchesAsync(patientId);
            return Ok(testBatches);
        }
        [HttpGet("LabWorker/batches")]
        [Authorize(Policy = "LabWorker")]
        public async Task<IActionResult> GetTestLabWorkerBatches()
        {
            Claim? labWorkerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (labWorkerIdClaim == null)
            {
                return Unauthorized(new { message = "No id in claims" });
            }
            int labWorkerId = int.Parse(labWorkerIdClaim.Value);
            List<TestBatchLabWorkerDTO> testBatches = await _patientsService.GetLabWorkerBatchesAsync(labWorkerId);
            return Ok(testBatches);
        }
        [HttpGet("batches/{batchId}")]
        [Authorize(Policy = "Patient")]
        public async Task<IActionResult> GetBatchResults(int batchId)
        {
            Claim? patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (patientIdClaim == null)
            {
                return Unauthorized(new { message = "No id in claims" });
            }
            int patientId = int.Parse(patientIdClaim.Value);
            BatchResultsDTO batchRes = await _patientsService.GetBatchResultsAsync(batchId);
            return Ok(batchRes);
        }
        [HttpGet]
        [Authorize(Policy = "Receptionist")]
        public async Task<IActionResult> GetPatients([FromQuery] string? fullName, [FromQuery] string? phone, [FromQuery] string? email, [FromQuery] string? dateOfBirth, [FromQuery] int page, [FromQuery] int pageSize)
        {
            if (string.IsNullOrWhiteSpace(fullName) && string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return BadRequest(new { message = "Either name, phone, email or date of birth must be provided." });
            }
            Claim? technicianIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (technicianIdClaim == null)
            {
                return Unauthorized(new { message = "No id in claims" });
            }
            int technicianId = int.Parse(technicianIdClaim.Value);
            ListWithTotalCount<PatientDTO> patients = await _patientsService.GetPatientListPagedAsync(fullName, phone, email, dateOfBirth, page, pageSize);
            
            if (patients.List.Count == 0)
            {
                return NotFound(new { message = "No patients found matching the criteria." });
            }
            return Ok(patients);
        }
        [HttpPost]
        [Authorize(Policy = "Receptionist")]
        public async Task<IActionResult> AddPatient([FromBody] PatientNew patient)
        {
            try
            {
                return Ok(new { id = await _patientsService.AddPatient(patient) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Addition failed. Check entry data and wether it already exists" });
            }
        }

        [HttpPost("submit-batch/{patientId}")]
        [Authorize(Policy = "Receptionist")]
        public async Task<IActionResult> SubmitBatchAsync(int patientId, [FromBody] NewOrder batch)
        {
            try
            {
                var technicianId = int.Parse(User.FindFirst("UserID").Value);
                await _patientsService.ValidateAndSubmitBatchesAsync(patientId, technicianId, batch);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
    }
}