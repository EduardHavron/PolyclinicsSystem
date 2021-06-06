using System.Threading.Tasks;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose
{
    public interface IDiagnoseService
    {
        public Task<GenerisResult<string, MedicalCardDto>> AddDiagnoseToCard(int appointmentId, int medicalCardId,
            string diagnose);

        public Task<GenerisResult<string, MedicalCardDto>> UpdateDiagnose(int diagnoseId, string diagnose);

        public Task<bool> DeleteDiagnose(int diagnoseId);
    }
}