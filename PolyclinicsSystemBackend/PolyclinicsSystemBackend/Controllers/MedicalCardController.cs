using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.MedicalCard;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("medCard")]
    public class MedicalCardController : ControllerBase
    {
        private readonly IMedicalCardService _medicalCardService;

        public MedicalCardController(
            IMedicalCardService medicalCardService)
        {
            _medicalCardService = medicalCardService ?? throw new ArgumentNullException(nameof(medicalCardService));
        }

        [HttpGet]
        [Route("get/{userId}")]
        public async Task<IActionResult> GetMedicalCard(string userId, bool includeDiagnoses = false)
        {
            var result = await _medicalCardService.GetMedicalCard(userId, includeDiagnoses);
            return result.IsSuccess ? Ok(result.Result) : NotFound(result.Errors);
        }

        [HttpPatch]
        [Route("update/{medicalCardId}")]
        public async Task<IActionResult> UpdateMedicalCard(int medicalCardId, [FromBody] MedicalCardDto medicalCard)
        {
            var result = await _medicalCardService.UpdateMedicalCard(medicalCard);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
    }
}