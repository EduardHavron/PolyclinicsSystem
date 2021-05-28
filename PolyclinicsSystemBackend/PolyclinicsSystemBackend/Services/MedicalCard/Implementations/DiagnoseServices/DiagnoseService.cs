using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations.DiagnoseServices
{
    public class DiagnoseService : IDiagnoseService
    {
        private readonly ILogger<DiagnoseService> _logger;
        private readonly AppDbContext _appDbContext;

        public DiagnoseService(ILogger<DiagnoseService> logger,
            AppDbContext appDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        
        public Task<Data.Entities.MedicalCard.MedicalCard?> AddDiagnoseToCard(int medicalCardId, Diagnose diagnose)
        {
            throw new System.NotImplementedException();
        }

        public Task<Data.Entities.MedicalCard.MedicalCard?> UpdateDiagnose(Diagnose diagnose)
        {
            throw new System.NotImplementedException();
        }

        public Task FinalizeDiagnose(int diagnoseId)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteDiagnose(int diagnoseId)
        {
            throw new System.NotImplementedException();
        }
    }
}