using System.Threading.Tasks;
using PolyclinicsSystemBackend.Dtos.Diagnose;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose
{
    public interface IDiagnoseService
    {
        public Task<GenerisResult<string, MedicalCardDto>> AddDiagnoseToCard(int appointmentId, DiagnoseDtoPost diagnoseDtoPost);

        public Task<GenerisResult<string, MedicalCardDto>> UpdateDiagnose(int diagnoseId, string diagnose);

        public Task<bool> DeleteDiagnose(int diagnoseId);
    }
}