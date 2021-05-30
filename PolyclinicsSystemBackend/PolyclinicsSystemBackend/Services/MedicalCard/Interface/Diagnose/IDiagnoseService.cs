using System.Threading.Tasks;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose
{
    public interface IDiagnoseService
    {
        public Task<Data.Entities.MedicalCard.MedicalCard?> AddDiagnoseToCard(int appointmentId, int medicalCardId,
            string diagnose);

        public Task<Data.Entities.MedicalCard.MedicalCard?> UpdateDiagnose(int diagnoseId, string diagnose);

        public Task<bool> DeleteDiagnose(int diagnoseId);
    }
}