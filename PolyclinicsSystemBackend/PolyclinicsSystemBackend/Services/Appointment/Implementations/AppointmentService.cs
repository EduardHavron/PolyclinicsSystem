using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Services.Appointment.Interface;

namespace PolyclinicsSystemBackend.Services.Appointment.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<AppointmentService> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager; 

        public AppointmentService(ILogger<AppointmentService> logger,
            AppDbContext appDbContext,
            UserManager<User> userManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Data.Entities.Appointment.Appointment?> GetAppointment(int appointmentId, bool includeDiagnose)
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
            if (appointment is not null) return appointment;
            _logger.LogError("No appointment with Id {Id} was found", appointmentId);
            return null;

        }

        public async Task<List<Data.Entities.Appointment.Appointment>?> GetAppointmentsForDoctor(string doctorId, bool includeDiagnose)
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
            if (appointments is not null) return appointments;
            _logger.LogError("No appointments for doctor with Id {Id} was founded", doctorId);
            return null;
        }

        public async Task<Data.Entities.Appointment.Appointment?> CreateAppointment(string doctorId, string patientId, DateTime appointmentDate)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            var doctorRoles = await _userManager.GetRolesAsync(doctor);
            if (doctor == null || !doctorRoles.Contains(Roles.Doctor.ToString()))
            {
                _logger.LogError("User with Id {Id} was not founded or it doesn't belong to role {Role}",
                    doctorId, Roles.Doctor.ToString());
                return null;
            }
            var patient = await _userManager.FindByIdAsync(patientId);
            var patientRoles = await _userManager.GetRolesAsync(patient);
            if (patient == null || !patientRoles.Contains(Roles.Patient.ToString()))
            {
                _logger.LogError("User with Id {Id} was not founded or it doesn't belong to role {Role}",
                    patientId, Roles.Patient.ToString());
                return null;
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
                return null;
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
            return result.Entity;
        }

        public async Task<Data.Entities.Appointment.Appointment?> RescheduleAppointment(int appointmentId, DateTime newDate)
        {
            _logger.LogInformation("Rescheduling appointment with Id {Id}", appointmentId);
            var appointment = await _appDbContext.Appointments
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return null;
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Planned)
            {
                _logger.LogInformation("Appointment is already started or already finalized");
                return null;
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
                return null;
            }
            _logger.LogInformation("Rescheduling appointment");
            appointment.AppointmentDate = strippedDate;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return appointment;
        }

        public async Task<Data.Entities.Appointment.Appointment?> StartAppointment(int appointmentId)
        {
            _logger.LogInformation("Marking appointment with Id {Id} as started", appointmentId);
            var appointment = await _appDbContext.Appointments
                .Include(appointmentEntity => appointmentEntity.Diagnose)
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return null;
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Planned)
            {
                _logger.LogInformation("Appointment is already started or already finalized");
                return null;
            }
            
            _logger.LogInformation("Changing status of appointment to started");
            appointment.AppointmentStatus = AppointmentStatuses.Started;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return appointment;
        }

        public async Task<Data.Entities.Appointment.Appointment?> FinalizeAppointment(int appointmentId)
        {
            _logger.LogInformation("Marking appointment with Id {Id} as finalized", appointmentId);
            var appointment = await _appDbContext.Appointments
                .Include(appointmentEntity => appointmentEntity.Diagnose)
                .FirstOrDefaultAsync(appointmentEntity => appointmentEntity.AppointmentId == appointmentId);
            if (appointment is null)
            {
                _logger.LogError("No appointment with Id {Id} was founded", appointmentId);
                return null;
            }

            if (appointment.AppointmentStatus != AppointmentStatuses.Started)
            {
                _logger.LogInformation("Appointment is already finalized or it's not started");
                return null;
            }
            
            _logger.LogInformation("Changing status of appointment to finalized");
            appointment.AppointmentStatus = AppointmentStatuses.Finalized;
            _logger.LogInformation("Saving changes");
            await _appDbContext.SaveChangesAsync();
            return appointment;
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