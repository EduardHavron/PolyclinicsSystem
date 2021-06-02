using System.Threading.Tasks;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard
{
    public interface IMedicalCardService
    {
        public Task<GenericResponse<string, MedicalCardDto>> GetMedicalCard(string userId, bool isDiagnoseIncluded);

        public Task<GenericResponse<string, MedicalCardDto>> CreateMedicalCard(string userId);

        public Task<GenericResponse<string, MedicalCardDto>> UpdateMedicalCard(
            MedicalCardDto medicalCard);
    }
}