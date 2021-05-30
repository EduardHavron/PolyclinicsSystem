using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Treatment;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Doctor")]
    [Route("treatment")]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService ?? throw new ArgumentNullException(nameof(treatmentService));
        }

        [HttpPost]
        [Route("{diagnoseId}")]
        public async Task<IActionResult> AddTreatmentToDiagnose(int diagnoseId, string treatment)
        {
            var result = await _treatmentService.AddTreatmentToDiagnose(diagnoseId, treatment);
            return result is null ? BadRequest() : Ok(result);
        }

        [HttpPatch]
        [Route("{treatmentId}")]
        public async Task<IActionResult> UpdateTreatment(int treatmentId, string treatment)
        {
            var result = await _treatmentService.UpdateTreatment(treatmentId, treatment);
            return result is null ? BadRequest() : Ok(result);
        }

        [HttpDelete]
        [Route("{treatmentId}")]
        public async Task<IActionResult> DeleteTreatment(int treatmentId)
        {
            var result = await _treatmentService.DeleteTreatment(treatmentId);
            return !result ? BadRequest() : Ok();
        }
    }
}