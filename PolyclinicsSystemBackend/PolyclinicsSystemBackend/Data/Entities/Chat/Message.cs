using System;

namespace PolyclinicsSystemBackend.Data.Entities.Chat
{
    public class Message
    {
        public int MessageId { get; set; }

        public int AppointmentId { get; set; }

        public Data.Entities.Appointment.Appointment Appointment { get; set; }

        public string SenderId { get; set; }

        public User.User Sender { get; set; }

        public string MessageContent { get; set; }

        public DateTime SendDate { get; set; }
    }
}