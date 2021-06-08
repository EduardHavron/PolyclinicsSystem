using System;
using System.Collections.Generic;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.Account.Doctor;
using PolyclinicsSystemBackend.Dtos.Account.Patient;
using PolyclinicsSystemBackend.Dtos.Chat;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Dtos.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatuses AppointmentStatus { get; set; }

        public DoctorDto Doctor { get; set; }

        public PatientDto Patient { get; set; }

        public DiagnoseDto? Diagnose { get; set; }

        public List<MessageDto> Messages { get; set; }
    }
}