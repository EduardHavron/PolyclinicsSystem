using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Dtos.Diagnose;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Doctor")]
    [Route("diagnose")]
    public class DiagnoseController : ControllerBase
    {
        private readonly IDiagnoseService _diagnoseService;

        public DiagnoseController(IDiagnoseService diagnoseService)
        {
            _diagnoseService = diagnoseService ?? throw new ArgumentNullException(nameof(diagnoseService));
        }

        [HttpPost]
        [Route("create/{appointmentId}")]
        public async Task<IActionResult> AddDiagnoseToCard(int appointmentId,[FromBody] DiagnoseDtoPost diagnoseDtoPost)
        {
            var result = await _diagnoseService.AddDiagnoseToCard(appointmentId, diagnoseDtoPost);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpPatch]
        [Route("update/{diagnoseId}")]
        public async Task<IActionResult> UpdateDiagnose(int diagnoseId, [FromBody] string diagnose)
        {
            var result = await _diagnoseService.UpdateDiagnose(diagnoseId, diagnose);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpDelete]
        [Route("delete/{diagnoseId}")]
        public async Task<IActionResult> DeleteDiagnose(int diagnoseId)
        {
            var result = await _diagnoseService.DeleteDiagnose(diagnoseId);
            return result ? Ok(result) : BadRequest(result);
        }
    }
}