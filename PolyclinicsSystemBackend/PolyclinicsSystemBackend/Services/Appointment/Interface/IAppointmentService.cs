using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Generics;

namespace PolyclinicsSystemBackend.Services.Appointment.Interface
{
    public interface IAppointmentService
    {
        public Task<GenerisResult<string, AppointmentDto>> GetAppointment(int appointmentId, bool includeDiagnose);

        public Task<GenerisResult<string, List<AppointmentDto>>> GetAppointmentsForDoctor(string doctorId,
            bool includeDiagnose);
        
        public Task<GenerisResult<string, AppointmentDto>> CreateAppointment(string doctorId, string patientId, DateTime appointmentDate);

        public Task<GenerisResult<string, AppointmentDto>> RescheduleAppointment(int appointmentId, DateTime newDate);

        public Task<GenerisResult<string, AppointmentDto>> StartAppointment(int appointmentId);

        public Task<GenerisResult<string, AppointmentDto>> FinalizeAppointment(int appointmentId);

        public Task<bool> CancelAppointment(int appointmentId);
        
    }
}