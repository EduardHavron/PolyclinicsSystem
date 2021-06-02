using System;

namespace PolyclinicsSystemBackend.Dtos.Chat
{
    public class MessageDto
    {
        public int MessageId { get; set; }

        public string SenderId { get; set; }

        public string MessageContent { get; set; }

        public DateTime SendDate { get; set; }
    }
}