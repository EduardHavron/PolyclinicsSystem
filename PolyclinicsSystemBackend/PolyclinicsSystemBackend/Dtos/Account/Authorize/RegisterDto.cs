using System.ComponentModel.DataAnnotations;

namespace PolyclinicsSystemBackend.Dtos.Account.Authorize
{
  public class RegisterDto
    {
      [Required]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "Password length must be between 6 and 100 symbols", MinimumLength = 6)]
      public string Password { get; set; }
    }
}