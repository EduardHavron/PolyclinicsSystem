using System;
using PolyclinicsSystemBackend.Data.Entities;

namespace PolyclinicsSystemBackend.Dtos.Chat
{
    public class Message
    {
        public int MessageId { get; set; }

        public int AppointmentId { get; set; }

        public Appointment.Appointment Appointment { get; set; }

        public string SenderId { get; set; }

        public User Sender { get; set; }

        public string MessageContent { get; set; }

        public DateTime SendDate { get; set; }
    }
}