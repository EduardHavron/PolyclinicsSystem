using System;
using System.Collections.Generic;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.Chat;

namespace PolyclinicsSystemBackend.Dtos.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatuses AppointmentStatus { get; set; }

        public string DoctorId { get; set; }

        public string PatientId { get; set; }

        public Diagnose? Diagnose { get; set; }

        public List<MessageDto> Messages { get; set; }
    }
}