using System;
using System.Collections.Generic;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Chat;

namespace PolyclinicsSystemBackend.Data.Entities.Appointment
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatuses AppointmentStatus { get; set; }

        public string DoctorId { get; set; }

        public User Doctor { get; set; }

        public string PatientId { get; set; }

        public User Patient { get; set; }

        public int? DiagnoseId { get; set; }

        public Diagnose? Diagnose { get; set; }

        public List<Message> Messages { get; set; }
    }
}