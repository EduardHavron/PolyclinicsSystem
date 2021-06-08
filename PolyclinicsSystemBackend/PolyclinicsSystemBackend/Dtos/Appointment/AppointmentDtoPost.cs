using System;

namespace PolyclinicsSystemBackend.Dtos.Appointment
{
    public class AppointmentDtoPost
    {
        public string DoctorId { get; set; }
        
        public string PatientId { get; set; }
        
        public DateTime AppointmentDate { get; set; }
    }
}