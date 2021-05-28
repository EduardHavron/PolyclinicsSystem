using System.Threading.Tasks;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard
{
    public interface IMedicalCardService
    {
        public Task<Data.Entities.MedicalCard.MedicalCard?> GetMedicalCard(string userId, bool isDiagnoseIncluded);

        public Task<Data.Entities.MedicalCard.MedicalCard> CreateMedicalCard(string userId);

        public Task<Data.Entities.MedicalCard.MedicalCard?> UpdateMedicalCard(
            Data.Entities.MedicalCard.MedicalCard medicalCard);
    }
}