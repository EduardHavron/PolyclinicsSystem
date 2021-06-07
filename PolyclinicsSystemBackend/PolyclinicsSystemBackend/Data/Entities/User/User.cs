using Microsoft.AspNetCore.Identity;

namespace PolyclinicsSystemBackend.Data.Entities.User
{
    public class User : IdentityUser
    {
        public string? DoctorType { get; set; }
        
        public string? Name { get; set; }
        
        public string? Surname { get; set; }
    }
}