using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.MedicalCard;
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
        [Route("create/{diagnoseId}")]
        public async Task<IActionResult> AddTreatmentToDiagnose(int diagnoseId,[FromBody]  TreatmentDtoPost treatment)
        {
            var result = await _treatmentService.AddTreatmentToDiagnose(diagnoseId, treatment.Treatment);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpPatch]
        [Route("update/{treatmentId}")]
        public async Task<IActionResult> UpdateTreatment(int treatmentId, [FromBody]  TreatmentDtoPost treatment)
        {
            var result = await _treatmentService.UpdateTreatment(treatmentId, treatment.Treatment);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpDelete]
        [Route("delete/{treatmentId}")]
        public async Task<IActionResult> DeleteTreatment(int treatmentId)
        {
            var result = await _treatmentService.DeleteTreatment(treatmentId);
            return result ? Ok() : BadRequest();
        }
    }
}