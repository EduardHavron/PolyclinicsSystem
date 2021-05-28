using System.Threading.Tasks;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Treatment
{
    public interface ITreatmentService
    {
        public Task<Data.Entities.MedicalCard.MedicalCard?> AddTreatmentToDiagnose(int diagnoseId, Data.Entities.MedicalCard.Treatment treatment);

        public Task<Data.Entities.MedicalCard.MedicalCard?> UpdateTreatment(Data.Entities.MedicalCard.Treatment treatment);

        public Task DeleteTreatment(int treatmentId);
    }
}