using System.ComponentModel.DataAnnotations;

namespace PolyclinicsSystemBackend.Dtos.Account.Authorize
{
  public class RegisterDto
    {
      public string Email { get; set; }
      public string Password { get; set; }
      
      public string FirstName { get; set; }
      
      public string LastName { get; set; }
      
      public Roles Role { get; set; }
    }
}