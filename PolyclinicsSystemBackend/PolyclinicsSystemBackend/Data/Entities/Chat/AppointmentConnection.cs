namespace PolyclinicsSystemBackend.Data.Entities.Chat
{
    public class AppointmentConnection
    {
        public int AppointmentConnectionId { get; set; }
        
        public string ConnectionId { get; set; }
        
        public int AppointmentGroupId { get; set; }
        
        public AppointmentGroup AppointmentGroup { get; set; }
    }
}