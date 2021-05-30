using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
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
        [Route("{userId}")]
        public async Task<IActionResult> GetMedicalCard(string userId, bool includeDiagnoses = false)
        {
            var result = await _medicalCardService.GetMedicalCard(userId, includeDiagnoses);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPatch]
        [Route("{medicalCardId}")]
        public async Task<IActionResult> UpdateMedicalCard(int medicalCardId, MedicalCard medicalCard)
        {
            var result = await _medicalCardService.UpdateMedicalCard(medicalCard);
            return result is null ? BadRequest() : Ok(result);
        }
    }
}