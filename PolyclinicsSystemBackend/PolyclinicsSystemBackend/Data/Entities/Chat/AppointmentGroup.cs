using System.Collections.Generic;

namespace PolyclinicsSystemBackend.Data.Entities.Chat
{
    public class AppointmentGroup
    {
        public int AppointmentGroupId { get; set; }
        
        public string AppointmentGroupName { get; set; }
        
        public List<AppointmentConnection> AppointmentGroups { get; set; }
    }
}