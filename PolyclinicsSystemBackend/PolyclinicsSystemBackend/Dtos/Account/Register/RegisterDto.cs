using PolyclinicsSystemBackend.HelperEntities;

namespace PolyclinicsSystemBackend.Dtos.Account.Register
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Roles Role { get; set; }

        public string DoctorType { get; set; }
    }
}