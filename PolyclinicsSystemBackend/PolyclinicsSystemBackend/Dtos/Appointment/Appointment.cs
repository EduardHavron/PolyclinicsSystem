using System;
using System.Collections.Generic;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Dtos.Chat;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Dtos.Appointment
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        
        public DateTime AppointmentDate { get; set; }
        
        public AppointmentStatuses IsFinalized { get; set; }
        
        public string DoctorId { get; set; }
        
        public User Doctor { get; set; }
        
        public string PatientId { get; set; }
        
        public User Patient { get; set; }

        public int? DiagnoseId { get; set; }
        
        public Diagnose? Diagnose { get; set; }
        
        public List<Message> Messages { get; set; }
    }
}