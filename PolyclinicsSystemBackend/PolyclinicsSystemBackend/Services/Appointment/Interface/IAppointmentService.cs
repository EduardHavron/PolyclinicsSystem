using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolyclinicsSystemBackend.Services.Appointment.Interface
{
    public interface IAppointmentService
    {
        public Task<Data.Entities.Appointment.Appointment?> GetAppointment(int appointmentId, bool includeDiagnose);

        public Task<List<Data.Entities.Appointment.Appointment>?> GetAppointmentsForDoctor(string doctorId,
            bool includeDiagnose);

        public Task<Data.Entities.Appointment.Appointment?> CreateAppointment(string doctorId, string patientId, DateTime appointmentDate);

        public Task<Data.Entities.Appointment.Appointment?> RescheduleAppointment(int appointmentId, DateTime newDate);

        public Task<Data.Entities.Appointment.Appointment?> StartAppointment(int appointmentId);

        public Task<Data.Entities.Appointment.Appointment?> FinalizeAppointment(int appointmentId);

        public Task<bool> CancelAppointment(int appointmentId);
    }
}