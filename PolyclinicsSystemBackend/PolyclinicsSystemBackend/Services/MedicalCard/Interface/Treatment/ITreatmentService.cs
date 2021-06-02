using System.Threading.Tasks;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Treatment
{
    public interface ITreatmentService
    {
        public Task<GenericResponse<string, MedicalCardDto>> AddTreatmentToDiagnose(int diagnoseId, string treatment);

        public Task<GenericResponse<string, MedicalCardDto>> UpdateTreatment(int treatmentId, string treatment);

        public Task<bool> DeleteTreatment(int treatmentId);
    }
}