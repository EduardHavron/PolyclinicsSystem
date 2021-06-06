using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Data.Entities.User;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.HelperEntities;
using PolyclinicsSystemBackend.Services.Appointment.Interface;

namespace PolyclinicsSystemBackend.Services.Appointment.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<AppointmentService> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AppointmentService(ILogger<AppointmentService> logger,
            AppDbContext appDbContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GenerisResult<string, AppointmentDto>> GetAppointment(int appointmentId, bool includeDiagnose)
        {
            _logger.LogInformation("Getting appointment with Id {Id}", appointmentId);
            var appointment = includeDiagnose
                ? await _appDbContext.Appointments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId)
                : await _appDbContext.Appointments
                    .AsNoTracking()
                    .Include(appointmentEntity => appointmentEntity.Diagnose)
                    .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is not null)
                return new GenerisResult<string, AppointmentDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<AppointmentDto>(appointment)
            };
            _logger.LogError("No appointment with Id {Id} was found", appointmentId);
            return new GenerisResult<string, AppointmentDto>
            {
                IsSuccess = false,
                Errors = new[]{$"No appointment with Id {appointmentId} was found"}
            };

        }

        public async Task<GenerisResult<string, List<AppointmentDto>>> GetAppointmentsForDoctor(string doctorId, bool includeDiagnose)
        {
            _logger.LogInformation("Getting all appointments for doctor with Id {Id}", doctorId);
            var appointments = includeDiagnose
                ? await _appDbContext.Appointments
                    .AsNoTracking()
                    .Where(appointmentEntity => appointmentEntity.DoctorId == doctorId)
                    .ToListAsync()
                : await _appDbContext.Appointments
                    .AsNoTracking()
                    .Include(appointmentEntity => appointmentEntity.Diagnose)
                    .Where(appointmentEntity => appointmentEntity.DoctorId == doctorId)
                    .ToListAsync();
            if (appointments is not null) return
                new GenerisResult<string, List<AppointmentDto>>
                {
                    IsSuccess = true,
                    Result = _mapper.Map<List<AppointmentDto>>(appointments)
                };
            _logger.LogError("No appointments for doctor with Id {Id} was founded", doctorId);
            return new GenerisResult<string, List<AppointmentDto>>
            {
                IsSuccess = false,
                Errors = new []{$"No appointments for doctor with Id {doctorId} was founded"}
            };
        }

        public async Task<GenerisResult<string, AppointmentDto>> CreateAppointment(string doctorId, string patientId, DateTime appointmentDate)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            var doctorRoles = await _userManager.GetRolesAsync(doctor);
            if (doctor == null || !doctorRoles.Contains(Roles.Doctor.ToString()))
            {
                _logger.LogError("User with Id {Id} was not founded or it doesn't belong to role {Role}",
                    doctorId, Roles.Doctor.ToString());
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"User with Id {doctorId} was not founded or it doesn't belong to role {Roles.Doctor.ToString()}"}
                };
            }
            var patient = await _userManager.FindByIdAsync(patientId);
            var patientRoles = await _userManager.GetRolesAsync(patient);
            if (patient == null || !patientRoles.Contains(Roles.Patient.ToString()))
            {
                _logger.LogError("User with Id {Id} was not founded or it doesn't belong to role {Role}",
                    patientId, Roles.Patient.ToString());
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"User with Id {patientId} was not founded or it doesn't belong to role {Roles.Patient.ToString()}"}
                };
            }

            var strippedDate = appointmentDate.Date + new TimeSpan(appointmentDate.TimeOfDay.Hours,
                appointmentDate.TimeOfDay.Minutes,
                0);
            var strippedDateNow = DateTime.Now + new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes,
                0);
            if (strippedDate <= strippedDateNow)
            {
                _logger.LogError("Cannot set appointment earlier than now or for the same time");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"Cannot set appointment earlier than now or for the same time"}
                };
            }
            _logger.LogInformation("Checking if any appointments already exists with given datetime");
            var existingAppointment = await _appDbContext.Appointments
                .AnyAsync(appointmentEntity => 
                    appointmentEntity.DoctorId == doctorId &&(
                    appointmentEntity.AppointmentDate == strippedDate ||
                    appointmentEntity.AppointmentDate.AddMinutes(30) == strippedDate ||
                    appointmentEntity.AppointmentDate.AddMinutes(-30) == strippedDate));
            if (existingAppointment)
            {
                _logger.LogError("Appointment with given date is already setted for this doctor");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new[] {"Appointment with given date is already setted for this doctor"}
                };
            }
            var appointment = new Data.Entities.Appointment.Appointment
            {
                AppointmentDate = strippedDate,
                DoctorId = doctorId,
                PatientId = patientId,
                AppointmentStatus = AppointmentStatuses.Planned
            };
            _logger.LogInformation("Adding appointment to database");
            var result = await _appDbContext.Appointments.AddAsync(appointment);
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenerisResult<string, AppointmentDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<AppointmentDto>(result.Entity)
            };
        }

        public async Task<GenerisResult<string, AppointmentDto>> RescheduleAppointment(int appointmentId, DateTime newDate)
        {
            _logger.LogInformation("Rescheduling appointment with Id {Id}", appointmentId);
            var appointment = await _appDbContext.Appointments
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"No appointment with Id {appointmentId} was founded"}
                };
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Planned)
            {
                _logger.LogInformation("Appointment is already started or already finalized");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{"Appointment is already started or already finalized"}
                };
            }
            var strippedDate = newDate.Date + new TimeSpan(newDate.TimeOfDay.Hours,
                newDate.TimeOfDay.Minutes,
                0);
            var strippedDateNow = DateTime.Now + new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes,
                0);
            if (strippedDate <= strippedDateNow)
            {
                _logger.LogError("Cannot set appointment earlier than now or for the same time");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{"Cannot set appointment earlier than now or for the same time"}
                };
            }
            _logger.LogInformation("Checking if any appointments already exists with given datetime");
            var existingAppointment = await _appDbContext.Appointments
                .AnyAsync(appointmentEntity => 
                    appointmentEntity.DoctorId == appointment.DoctorId &&(
                        appointmentEntity.AppointmentDate == strippedDate ||
                        appointmentEntity.AppointmentDate.AddMinutes(30) == strippedDate ||
                        appointmentEntity.AppointmentDate.AddMinutes(-30) == strippedDate));
            if (existingAppointment)
            {
                _logger.LogError("Appointment with given date is already setted for this doctor");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new[] {"Appointment with given date is already setted for this doctor"}
                };
            }
            _logger.LogInformation("Rescheduling appointment");
            appointment.AppointmentDate = strippedDate;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenerisResult<string, AppointmentDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<AppointmentDto>(appointment)
            };
        }

        public async Task<GenerisResult<string, AppointmentDto>> StartAppointment(int appointmentId)
        {
            _logger.LogInformation("Marking appointment with Id {Id} as started", appointmentId);
            var appointment = await _appDbContext.Appointments
                .Include(appointmentEntity => appointmentEntity.Diagnose)
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"No appointment with Id {appointmentId} was founded"}
                };
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Planned)
            {
                _logger.LogInformation("Appointment is already started or already finalized");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{"Appointment is already started or already finalized"}
                };
            }
            
            _logger.LogInformation("Changing status of appointment to started");
            appointment.AppointmentStatus = AppointmentStatuses.Started;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenerisResult<string, AppointmentDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<AppointmentDto>(appointment)
            };
        }

        public async Task<GenerisResult<string, AppointmentDto>> FinalizeAppointment(int appointmentId)
        {
            _logger.LogInformation("Marking appointment with Id {Id} as finalized", appointmentId);
            var appointment = await _appDbContext.Appointments
                .Include(appointmentEntity => appointmentEntity.Diagnose)
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{$"No appointment with Id {appointmentId} was founded"}
                };
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogInformation("Appointment is already finalized or it's not started");
                return new GenerisResult<string, AppointmentDto>
                {
                    IsSuccess = false,
                    Errors = new []{"Appointment is already finalized or it's not started"}
                };
            }
            
            _logger.LogInformation("Changing status of appointment to finalized");
            appointment.AppointmentStatus = AppointmentStatuses.Finalized;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return new GenerisResult<string, AppointmentDto>
            {
                IsSuccess = true,
                Result = _mapper.Map<AppointmentDto>(appointment)
            };
        }

        public async Task<bool> CancelAppointment(int appointmentId)
        {
            _logger.LogInformation("Cancelling appointment with Id {Id}", appointmentId);
            var appointment = await _appDbContext.Appointments
                .Include(appointmentEntity => appointmentEntity.Diagnose)
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return false;
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Planned)
            {
                _logger.LogInformation("Appointment is already started or already finalized");
                return false;
            }
            
            _logger.LogInformation("Cancelling appointment");
            _appDbContext.Appointments.Remove(appointment);
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}