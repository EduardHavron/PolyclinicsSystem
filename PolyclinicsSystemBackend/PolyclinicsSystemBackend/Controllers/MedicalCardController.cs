using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Authorize]
    public class MedicalCardController : ControllerBase
    {
        private readonly IMedicalCardService _medicalCardService;

        public MedicalCardController(
            IMedicalCardService medicalCardService)
        {
            _medicalCardService = medicalCardService ?? throw new ArgumentNullException(nameof(medicalCardService));
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicalCard(string userId, bool includeDiagnoses = false)
        {
            var result = await _medicalCardService.GetMedicalCard(userId, includeDiagnoses);
            return result is null ? NotFound() : Ok(result);
        }
    }
}