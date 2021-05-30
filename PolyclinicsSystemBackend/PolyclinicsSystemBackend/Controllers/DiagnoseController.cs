using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Route("{appointmentId}")]
        public async Task<IActionResult> AddDiagnoseToCard(int appointmentId, int medicalCardId, string diagnose)
        {
            var result = await _diagnoseService.AddDiagnoseToCard(appointmentId, medicalCardId, diagnose);
            return result is null ? BadRequest() : Ok(result);
        }

        [HttpPatch]
        [Route("{diagnoseId}")]
        public async Task<IActionResult> UpdateDiagnose(int diagnoseId, string diagnose)
        {
            var result = await _diagnoseService.UpdateDiagnose(diagnoseId, diagnose);
            return result is null ? BadRequest() : Ok(result);
        }

        [HttpDelete]
        [Route("{diagnoseId}")]
        public async Task<IActionResult> DeleteDiagnose(int diagnoseId)
        {
            var result = await _diagnoseService.DeleteDiagnose(diagnoseId);
            return !result ? BadRequest() : Ok();
        }
    }
}