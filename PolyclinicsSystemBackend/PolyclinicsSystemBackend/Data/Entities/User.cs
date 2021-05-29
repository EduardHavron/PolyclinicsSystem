using Microsoft.AspNetCore.Identity;

namespace PolyclinicsSystemBackend.Data.Entities
{
  public class User : IdentityUser
  {
    public string? DoctorType { get; set; }
  }
}