using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Treatment;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations.TreatmentServices
{
    public class TreatmentService : ITreatmentService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<TreatmentService> _logger;

        public TreatmentService(ILogger<TreatmentService> logger,
            AppDbContext appDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard?> AddTreatmentToDiagnose(int diagnoseId,
            string treatment)
        {
            _logger.LogInformation("Adding treatment to diagnose with Id {Id}", diagnoseId);
            var diagnose =
                await _appDbContext.Diagnoses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(diagnoseEntity => diagnoseEntity.DiagnoseId == diagnoseId);
            if (diagnose == null)
            {
                _logger.LogError("No diagnose with Id {Id} was found in database", diagnoseId);
                return null;
            }

            if (treatment == string.Empty)
            {
                _logger.LogError("Treatment cannot be empty");
                return null;
            }

            var newTreatment = new Treatment
            {
                DiagnoseId = diagnoseId,
                TreatmentDate = DateTime.Now,
                TreatmentInstructions = treatment
            };
            _logger.LogInformation("Adding treatment to database");
            await _appDbContext.Treatments.AddAsync(newTreatment);
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return await _appDbContext.MedicalCards
                .AsNoTracking()
                .Include(medicalCardEntity => medicalCardEntity.Diagnoses)
                .ThenInclude(diagnoseEntity =>
                    diagnoseEntity.Treatment)
                .FirstOrDefaultAsync(medicalCardEntity => medicalCardEntity.MedicalCardId == diagnose.MedicalCardId);
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard?> UpdateTreatment(int treatmentId, string treatment)
        {
            _logger.LogInformation("Updating treatment with Id {Id}", treatmentId);
            var oldTreatment = await _appDbContext.Treatments
                .FirstOrDefaultAsync(treatmentEntity => treatmentEntity.TreatmentId == treatmentId);
            if (oldTreatment is null)
            {
                _logger.LogError("Treatment with Id {Id} doesn't exist in database", treatmentId);
                return null;
            }

            var associatedAppointment = await _appDbContext.Appointments
                .AsNoTracking()
                .FirstOrDefaultAsync(appointmentEntity =>
                    appointmentEntity.Diagnose.Treatment.TreatmentId == treatmentId);
            if (associatedAppointment is null)
            {
                _logger.LogError("No associated appointment with related diagnose was founded");
                return null;
            }
            if (associatedAppointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogError("Associated appointment with Id {Id} status is not {Status} ",
                    associatedAppointment.AppointmentId, AppointmentStatuses.Started.ToString());
            }
            _logger.LogInformation("Updating treatment");
            oldTreatment.TreatmentDate = DateTime.Now;
            oldTreatment.TreatmentInstructions = treatment;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return await _appDbContext.Treatments
                .AsNoTracking()
                .Where(treatmentEntity => 
                    treatmentEntity.Diagnose.Treatment != null &&
                    treatmentEntity.Diagnose.Treatment.TreatmentId == treatmentId)
                .Select(treatmentEntity => treatmentEntity.Diagnose.MedicalCard)
                .Include(medicalCardEntity => medicalCardEntity.Diagnoses)
                .ThenInclude(diagnoseEntity => diagnoseEntity.Treatment)
                .FirstOrDefaultAsync();
        }
        
        public async Task<bool> DeleteTreatment(int treatmentId)
        {
            _logger.LogInformation("Deleting treatment with Id {Id}", treatmentId);
            var associatedAppointment = await _appDbContext.Appointments
                .FirstOrDefaultAsync(appointmentEntity =>
                appointmentEntity.Diagnose.Treatment.TreatmentId == treatmentId);
            if (associatedAppointment is null)
            {
                _logger.LogError("Appointment associated with treatment doesn't exist");
                return false;
            }

            if (associatedAppointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogError("Associated appointment with Id {Id} status is not {Status} ",
                    associatedAppointment.AppointmentId, AppointmentStatuses.Started.ToString());
            }

            var treatmentToDelete =
                await _appDbContext.Treatments
                    .FirstOrDefaultAsync(treatmentEntity =>
                    treatmentEntity.TreatmentId == treatmentId);
            if (treatmentToDelete is null)
            {
                _logger.LogError("Treatment with Id {Id} wasn't founded in database", treatmentId);
                return false;
            }
            _logger.LogInformation("Deleting treatment");
            _appDbContext.Treatments.Remove(treatmentToDelete);
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}