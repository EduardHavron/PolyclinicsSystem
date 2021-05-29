using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Treatment;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations.TreatmentServices
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ILogger<TreatmentService> _logger;
        private readonly AppDbContext _appDbContext;

        public TreatmentService(ILogger<TreatmentService> logger,
            AppDbContext appDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        
        public async Task<Data.Entities.MedicalCard.MedicalCard?> AddTreatmentToDiagnose(int diagnoseId, Treatment treatment)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard?> UpdateTreatment(Treatment treatment)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteTreatment(int treatmentId)
        {
            throw new System.NotImplementedException();
        }
    }
}