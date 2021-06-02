using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations.MedicalCardServices
{
    public class MedicalCardService : IMedicalCardService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<MedicalCardService> _logger;
        private readonly IMapper _mapper;

        public MedicalCardService(AppDbContext appDbContext,
            ILogger<MedicalCardService> logger,
            IMapper mapper)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<GenericResponse<string, MedicalCardDto>> GetMedicalCard(string userId, bool isDiagnoseIncluded)
        {
            _logger.LogInformation("Getting medical card for user with Id {Id}.\n" +
                                   "Including diagnoses: {IncludingDiagnoses}",
                userId, isDiagnoseIncluded);

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User Id was null or empty!");
                return new GenericResponse<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{"User Id was null or empty!"}
                };
            }

            var medicalCard = await _appDbContext
                .MedicalCards
                .AsNoTracking()
                .Include(medCard => medCard.Diagnoses
                    .Where(diagnose => isDiagnoseIncluded)
                    .OrderByDescending(diagnose => diagnose.DiagnoseDate))
                .ThenInclude(
                    diagnose => diagnose.Treatment)
                .FirstOrDefaultAsync(medCard => medCard.UserId == userId);
            if (medicalCard is null)
            {
                _logger.LogError("Medical card associated with user wasn't founded");
                return new GenericResponse<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{"Medical card associated with user wasn't founded"}
                };
            }
            _logger.LogInformation("Successfully retrieved medical card for user {UserId}", userId);
            return new GenericResponse<string, MedicalCardDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<MedicalCardDto>(medicalCard)
            };
        }

        public async Task<GenericResponse<string, MedicalCardDto>> CreateMedicalCard(string userId)
        {
            _logger.LogInformation("Creating empty medical card for user {UserId}", userId);
            var result = await _appDbContext.MedicalCards.AddAsync(new Data.Entities.MedicalCard.MedicalCard
                {UserId = userId});
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenericResponse<string, MedicalCardDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<MedicalCardDto>(result.Entity)
            };
        }

        public async Task<GenericResponse<string, MedicalCardDto>> UpdateMedicalCard(
            MedicalCardDto medicalCard)
        {
            _logger.LogInformation("Updating medical card with Id {Id}", medicalCard.MedicalCardId);
            var oldMedicalCard = await _appDbContext.MedicalCards
                .FirstOrDefaultAsync(medCard =>
                    medCard.MedicalCardId == medicalCard.MedicalCardId);
            if (oldMedicalCard is null)
            {
                _logger.LogError("Medical card with Id {Id} wasn't founded", medicalCard.MedicalCardId);
                return new GenericResponse<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"Medical card with Id {medicalCard.MedicalCardId} wasn't founded"}
                };
            }
            _logger.LogInformation("Updating medical card");
            oldMedicalCard.Age = medicalCard.Age;
            oldMedicalCard.Height = medicalCard.Height;
            oldMedicalCard.Weight = medicalCard.Weight;
            oldMedicalCard.AdditionalInfo = medicalCard.AdditionalInfo;
            oldMedicalCard.Gender = medicalCard.Gender;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenericResponse<string, MedicalCardDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<MedicalCardDto>(oldMedicalCard)
            };
        }
    }
}