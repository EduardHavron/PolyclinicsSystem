using System.Threading.Tasks;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard
{
    public interface IMedicalCardService
    {
        public Task<GenerisResult<string, MedicalCardDto>> GetMedicalCard(string userId, bool isDiagnoseIncluded);

        public Task<GenerisResult<string, MedicalCardDto>> CreateMedicalCard(string userId);

        public Task<GenerisResult<string, MedicalCardDto>> UpdateMedicalCard(
            MedicalCardDto medicalCard);
    }
}