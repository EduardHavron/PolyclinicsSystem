using System.Threading.Tasks;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Interface.Treatment
{
    public interface ITreatmentService
    {
        public Task<Data.Entities.MedicalCard.MedicalCard?> AddTreatmentToDiagnose(int diagnoseId, string treatment);

        public Task<Data.Entities.MedicalCard.MedicalCard?> UpdateTreatment(int treatmentId, string treatment);

        public Task<bool> DeleteTreatment(int treatmentId);
    }
}