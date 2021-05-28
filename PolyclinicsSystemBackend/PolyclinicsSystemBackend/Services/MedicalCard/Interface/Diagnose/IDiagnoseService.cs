using System.Threading.Tasks;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose
{
    public interface IDiagnoseService
    {
        
        public Task<Data.Entities.MedicalCard.MedicalCard?> AddDiagnoseToCard(int medicalCardId, Data.Entities.MedicalCard.Diagnose diagnose);

        public Task<Data.Entities.MedicalCard.MedicalCard?> UpdateDiagnose(Data.Entities.MedicalCard.Diagnose diagnose);

        public Task FinalizeDiagnose(int diagnoseId);

        public Task DeleteDiagnose(int diagnoseId);
    }
}