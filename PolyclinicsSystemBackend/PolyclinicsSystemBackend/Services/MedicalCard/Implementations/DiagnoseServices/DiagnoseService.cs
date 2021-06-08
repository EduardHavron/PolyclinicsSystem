using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Diagnose;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Dtos.MedicalCard;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.Diagnose;

namespace PolyclinicsSystemBackend.Services.MedicalCard.Implementations.DiagnoseServices
{
    public class DiagnoseService : IDiagnoseService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<DiagnoseService> _logger;
        private readonly IMapper _mapper;
        
        public DiagnoseService(ILogger<DiagnoseService> logger,
            AppDbContext appDbContext,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GenerisResult<string, MedicalCardDto>> AddDiagnoseToCard(int appointmentId,
            DiagnoseDtoPost diagnoseDtoPost)
        {
            _logger.LogInformation("Adding diagnose to med card with Id {Id} using appointment with Id {AppointmentId}"
                , diagnoseDtoPost.MedicalCardId, appointmentId);

            if (diagnoseDtoPost.Diagnose == string.Empty)
            {
                _logger.LogError("Diagnose cannot be empty");
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new[] {"Diagnose cannot be empty"}
                };
            }

            var appointment = await _appDbContext.Appointments
                .FirstOrDefaultAsync(appointmentEntity =>
                appointmentEntity.AppointmentId == appointmentId);
            if (appointment == null)
            {
                _logger.LogError("Appointment with Id {Id} doesn't exist in database", appointmentId);
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new[] {$"Appointment with Id {appointmentId} doesn't exist in database"}
                };
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogError("Cannot add diagnose to appointment with status {Status}",
                    appointment.AppointmentStatus.ToString());
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new[] {$"Cannot add diagnose to appointment with status {appointment.AppointmentStatus.ToString()}"}
                };
            }

            var medicalCardExist =
                await _appDbContext.MedicalCards
                    .AsNoTracking()
                    .AnyAsync(medCard => medCard.MedicalCardId == diagnoseDtoPost.MedicalCardId);
            if (medicalCardExist == false)
            {
                _logger.LogError("Medical card with Id {Id} doesn't exist in database", diagnoseDtoPost.MedicalCardId);
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new[] {$"Medical card with Id {diagnoseDtoPost.MedicalCardId} doesn't exist in database"}
                };
            }
            var diagnoseObject = new Diagnose
            {
                DiagnoseDate = DateTime.Now,
                DiagnoseInfo = diagnoseDtoPost.Diagnose,
                MedicalCardId = diagnoseDtoPost.MedicalCardId
            };
            _logger.LogInformation("Adding diagnose to database");
            var result = await _appDbContext.Diagnoses.AddAsync(diagnoseObject);

            _logger.LogInformation("Attaching diagnose to appointment");
            appointment.DiagnoseId = result.Entity.DiagnoseId;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenerisResult<string, MedicalCardDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<MedicalCardDto>(await _appDbContext.MedicalCards
                    .AsNoTracking()
                    .Include(medCard => medCard.Diagnoses)
                    .ThenInclude(diagnoseEntity =>
                        diagnoseEntity.Treatment)
                    .FirstOrDefaultAsync(medCard =>
                        medCard.MedicalCardId == diagnoseDtoPost.MedicalCardId))
            };
        }

        public async Task<GenerisResult<string, MedicalCardDto>> UpdateDiagnose(int diagnoseId, string diagnose)
        {
            _logger.LogInformation("Updating diagnose with Id {Id}", diagnoseId);
            var oldDiagnose = await _appDbContext.Diagnoses
                .FirstOrDefaultAsync(diagnoseEntity =>
                    diagnoseEntity.DiagnoseId == diagnoseId);
            if (oldDiagnose == null)
            {
                _logger.LogError("Diagnose with Id {Id} doesn't exist", diagnoseId);
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"Diagnose with Id {diagnoseId} doesn't exist"}
                };
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
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{"No appointment associated with diagnose found"}
                };
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogError("Cannot update diagnose for appointment with status {Status}",
                    appointment.AppointmentStatus.ToString());
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"Cannot update diagnose for appointment with status {appointment.AppointmentStatus.ToString()}"}
                };
            }

            if (diagnose == string.Empty)
            {
                _logger.LogError("Diagnose cannot be empty");
                return new GenerisResult<string, MedicalCardDto>
                {
                    IsSuccess = false,
                    Errors = new []{"Diagnose cannot be empty"}
                };
            }
            _logger.LogInformation("Updating diagnose");
            oldDiagnose.DiagnoseDate = DateTime.Now;
            oldDiagnose.DiagnoseInfo = diagnose;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenerisResult<string, MedicalCardDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<MedicalCardDto>( await _appDbContext.MedicalCards
                    .AsNoTracking()
                    .Include(medCard => medCard.Diagnoses)
                    .ThenInclude(diagnoseEntity =>
                        diagnoseEntity.Treatment)
                    .FirstOrDefaultAsync(medCard =>
                        medCard.MedicalCardId == oldDiagnose.MedicalCardId))
            };
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

            if (appointment != null && appointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogError("Cannot delete diagnose where associated appointment has status {Status}",
                    appointment.AppointmentStatus.ToString());
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