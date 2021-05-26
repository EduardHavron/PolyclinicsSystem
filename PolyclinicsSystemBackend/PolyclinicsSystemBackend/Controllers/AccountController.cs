using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PolyclinicsSystemBackend.Config.Options;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;

namespace PolyclinicsSystemBackend.Controllers
{
  // TODO: Move logic to service
  [ApiController]
  [Route("account")]
  public class AccountController : ControllerBase
  {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<AuthOptions> _options;

    public AccountController(
      UserManager<User> userManager,
      SignInManager<User> signInManager,
      RoleManager<IdentityRole> roleManager,
      IOptions<AuthOptions> options
    )
    {
      _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
      _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
      _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
      _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    [HttpPost("authorize")]
    public async Task<object> Authorize([FromBody] AuthorizeDto model)
    {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

      if (!result.Succeeded) throw new BusinessLogicException($"Failed to authorize user, reason: {result}");
      var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
      var token = await GenerateJwtToken(model.Email, appUser ?? throw new BusinessLogicException("Results is succeeded but user doesn't founded"));
      return new {token};
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
      var user = new User
      {
        UserName = model.Email,
        Email = model.Email
      };
      var result = await _userManager.CreateAsync(user, model.Password);
      //_roleManager.
      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, false);
      }
    
      return Ok();
    }

    private async Task<string> GenerateJwtToken(string email, User user)
    {
      var claims = new List<Claim>
      {
        new (JwtRegisteredClaimNames.Sub, email),
        new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new (ClaimTypes.NameIdentifier, user.Id),
      };
      var userRoles = await _userManager.GetRolesAsync(user);
      claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expires = DateTime.Now.AddDays(Convert.ToDouble(_options.Value.ExpireDays));

      var token = new JwtSecurityToken(
        _options.Value.Issuer,
        _options.Value.Issuer,
        claims,
        expires: expires,
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}