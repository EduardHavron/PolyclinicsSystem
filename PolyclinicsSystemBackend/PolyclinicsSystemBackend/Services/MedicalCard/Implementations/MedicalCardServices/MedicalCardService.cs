using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations
{
    public class MedicalCardService : IMedicalCardService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<MedicalCardService> _logger;
        public MedicalCardService(AppDbContext appDbContext,
            ILogger<MedicalCardService> logger)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        
        public async Task<Data.Entities.MedicalCard.MedicalCard?> GetMedicalCard(string userId, bool isDiagnoseIncluded)
        {
            _logger.LogInformation("Getting medical card for user with Id {Id}.\n" +
                                   "Including diagnoses: {IncludingDiagnoses}",
                userId, isDiagnoseIncluded);
            
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User Id was null or empty!");
                return null;
            }

            var medicalCard = await  _appDbContext
                .MedicalCards
                .Include(medCard => medCard.Diagnoses
                    .Where(diagnose => isDiagnoseIncluded)
                    .OrderByDescending(diagnose => diagnose.DiagnoseDate))
                .ThenInclude(diagnose => diagnose.Treatment)
                .AsNoTracking()
                .FirstOrDefaultAsync(medCard => medCard.UserId == userId);

            _logger.LogInformation("Successfully retrieved medical card for user {UserId}",userId);
            return medicalCard;
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard> CreateMedicalCard(string userId)
        {
            _logger.LogInformation("Creating empty medical card for user {UserId}", userId);
             var result = await _appDbContext.MedicalCards.AddAsync(new Data.Entities.MedicalCard.MedicalCard {UserId = userId});
             await _appDbContext.SaveChangesAsync();
             return result.Entity;
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard?> UpdateMedicalCard(Data.Entities.MedicalCard.MedicalCard medicalCard)
        {
            if (medicalCard.MedicalCardId == 0)
            {
                _logger.LogError("Cannot update medical card without Id");
                return null;
            }
            _logger.LogInformation("Updating medical card with Id {Id}", medicalCard.MedicalCardId);
            var oldMedicalCard = await _appDbContext.MedicalCards
                .FirstOrDefaultAsync(medCard =>
                medCard.MedicalCardId == medicalCard.MedicalCardId);
            if (oldMedicalCard is null)
            {
                _logger.LogError("Medical card with Id {Id} wasn't founded", medicalCard.MedicalCardId);
                return null;
            }
            
            oldMedicalCard.Age = medicalCard.Age;
            oldMedicalCard.Height = medicalCard.Height;
            oldMedicalCard.Weight = medicalCard.Weight;
            oldMedicalCard.AdditionalInfo = medicalCard.AdditionalInfo;
            oldMedicalCard.Gender = medicalCard.Gender;
            await _appDbContext.SaveChangesAsync();
            return oldMedicalCard;
        }
    }
}