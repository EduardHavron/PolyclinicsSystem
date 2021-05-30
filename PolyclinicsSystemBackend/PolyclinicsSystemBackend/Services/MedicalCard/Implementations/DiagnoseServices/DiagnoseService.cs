using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations.DiagnoseServices
{
    public class DiagnoseService : IDiagnoseService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<DiagnoseService> _logger;

        public DiagnoseService(ILogger<DiagnoseService> logger,
            AppDbContext appDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard?> AddDiagnoseToCard(int appointmentId,
            int medicalCardId, string diagnose)
        {
            _logger.LogInformation("Adding diagnose to med card with Id {Id} using appointment with Id {AppointmentId}"
                , medicalCardId, appointmentId);

            if (diagnose == string.Empty)
            {
                _logger.LogError("Diagnose cannot be empty");
                return null;
            }

            var appointment = await _appDbContext.Appointments
                .FirstOrDefaultAsync(appointmentEntity =>
                appointmentEntity.AppointmentId == appointmentId);
            if (appointment == null)
            {
                _logger.LogError("Appointment with Id {Id} doesn't exist in database", appointmentId);
                return null;
            }

            if (appointment.IsFinalized != AppointmentStatuses.Started)
            {
                _logger.LogError("Cannot add diagnose to appointment with status {Status}",
                    appointment.IsFinalized.ToString());
                return null;
            }

            var medicalCardExist =
                await _appDbContext.MedicalCards
                    .AsNoTracking()
                    .AnyAsync(medCard => medCard.MedicalCardId == medicalCardId);
            if (medicalCardExist == false)
                _logger.LogError("Medical card with Id {Id} doesn't exist in database", medicalCardId);

            var diagnoseObject = new Diagnose
            {
                DiagnoseDate = DateTime.Now,
                DiagnoseInfo = diagnose,
                MedicalCardId = medicalCardId
            };
            _logger.LogInformation("Adding diagnose to database");
            var result = await _appDbContext.Diagnoses.AddAsync(diagnoseObject);

            _logger.LogInformation("Attaching diagnose to appointment");
            appointment.DiagnoseId = result.Entity.DiagnoseId;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return await _appDbContext.MedicalCards
                .AsNoTracking()
                .Include(medCard => medCard.Diagnoses)
                .ThenInclude(diagnoseEntity =>
                    diagnoseEntity.Treatment)
                .FirstOrDefaultAsync(medCard =>
                    medCard.MedicalCardId == medicalCardId);
        }

        public async Task<Data.Entities.MedicalCard.MedicalCard?> UpdateDiagnose(int diagnoseId, string diagnose)
        {
            _logger.LogInformation("Updating diagnose with Id {Id}", diagnoseId);
            var oldDiagnose = await _appDbContext.Diagnoses
                .FirstOrDefaultAsync(diagnoseEntity =>
                    diagnoseEntity.DiagnoseId == diagnoseId);
            if (oldDiagnose == null)
            {
                _logger.LogError("Diagnose with Id {Id} doesn't exist", diagnoseId);
                return null;
            }

            _logger.LogInformation("Retrieving linked appointment");
            var appointment =
                await _appDbContext.Appointments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        appointmentEntity => appointmentEntity.DiagnoseId == diagnoseId);
            if (appointment == null)
            {
                _logger.LogError("No appointment associated with diagnose found");
                return null;
            }

            if (appointment.IsFinalized != AppointmentStatuses.Started)
            {
                _logger.LogError("Cannot update diagnose for appointment with status {Status}",
                    appointment.IsFinalized.ToString());
                return null;
            }

            if (diagnose == string.Empty)
            {
                _logger.LogError("Diagnose cannot be empty");
                return null;
            }
            _logger.LogInformation("Updating diagnose");
            oldDiagnose.DiagnoseDate = DateTime.Now;
            oldDiagnose.DiagnoseInfo = diagnose;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return await _appDbContext.MedicalCards
                .AsNoTracking()
                .Include(medCard => medCard.Diagnoses)
                .ThenInclude(diagnoseEntity =>
                    diagnoseEntity.Treatment)
                .FirstOrDefaultAsync(medCard =>
                    medCard.MedicalCardId == oldDiagnose.MedicalCardId);
        }

        public async Task<bool> DeleteDiagnose(int diagnoseId)
        {
            _logger.LogInformation("Trying to delete diagnose with Id {Id}", diagnoseId);
            var diagnose =
                await _appDbContext.Diagnoses.FirstOrDefaultAsync(diagnoseEntity =>
                    diagnoseEntity.DiagnoseId == diagnoseId);
            if (diagnose == null)
            {
                _logger.LogError("No diagnose with Id {Id} was founded", diagnoseId);
                return false;
            }

            var appointment =
                await _appDbContext.Appointments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(appointmentEntity =>
                    appointmentEntity.DiagnoseId == diagnoseId);

            if (appointment != null && appointment.IsFinalized != AppointmentStatuses.Started)
            {
                _logger.LogError("Cannot delete diagnose where associated appointment has status {Status}",
                    appointment.IsFinalized.ToString());
                return false;
            }
            _logger.LogInformation("Removing diagnose from database");
            _appDbContext.Diagnoses.Remove(diagnose);
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}