using System.ComponentModel.DataAnnotations;

namespace PolyclinicsSystemBackend.Dtos.Account.Authorize
{
    public class AuthorizeDto
    {
      [Required]
      public string Email { get; set; }

      [Required]
      public string Password { get; set; }
    }
}