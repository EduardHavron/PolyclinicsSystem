using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolyclinicsSystemBackend.Config.Options;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Exceptions;
using PolyclinicsSystemBackend.Services.Account.Interface;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

namespace PolyclinicsSystemBackend.Services.Account.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<AuthOptions> _options;
        private readonly ILogger<AccountService> _logger;
        private readonly IMedicalCardService _medicalCardService;

        public AccountService(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<AuthOptions> options,
        ILogger<AccountService> logger,
        IMedicalCardService medicalCardService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _medicalCardService = medicalCardService ?? throw new ArgumentNullException(nameof(medicalCardService));
        }

        public async Task<AuthorizeModel> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };
            
            _logger.LogInformation("Creating user with following email {User}", user.Email);
            
            var registerResult = await _userManager.CreateAsync(user, registerDto.Password);
            if (!registerResult.Succeeded)
            {
                _logger.LogInformation("Registration failed. Reasons: {Reasons}",
                    registerResult.Errors.Select(x => x.Description));
                return new AuthorizeModel {Errors = registerResult.Errors};
            }
            var assignRoleResult = await _userManager.AddToRoleAsync(user, registerDto.Role.ToString());
            if (assignRoleResult.Succeeded)
            {
                if (registerDto.Role == Roles.Patient)
                {
                    _logger.LogInformation("Creating a med card for patient with email {Email}", user.Email);
                    var userId = await GetUserId(user.Email);
                    if (userId == null)
                    {
                        return new AuthorizeModel
                        {
                            Errors = new List<IdentityError>
                            {
                                new IdentityErrorDescriber().InvalidEmail(user.Email) 
                            }
                        };
                    }
                    await _medicalCardService.CreateMedicalCard(userId);
                }
                return await Authorize(new AuthorizeDto {Email = registerDto.Email, Password = registerDto.Password});
            }
            _logger.LogInformation("Adding to role failed. Reasons: {Reasons}",
                    assignRoleResult.Errors.Select(x => x.Description));
                return new AuthorizeModel {Errors = assignRoleResult.Errors};
        }

        public async Task<AuthorizeModel> Authorize(AuthorizeDto authorizeDto)
        {
            _logger.LogInformation("Logging in user with email: {Email}", authorizeDto.Email);
            var result = await _signInManager.PasswordSignInAsync(authorizeDto.Email, authorizeDto.Password, false, false);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Failed to login user");
                return new AuthorizeModel
                {
                    Errors = new List<IdentityError>
                    {
                        new IdentityErrorDescriber().PasswordMismatch()
                    }
                };
            }
            var appUser = _userManager.Users.AsNoTracking().SingleOrDefault(r => r.Email == authorizeDto.Email);
            var token = await GenerateJwtToken(authorizeDto.Email, appUser ?? throw new BusinessLogicException("Results is succeeded but user doesn't founded"));
            return new AuthorizeModel{Succeeded = true, Token = token};
        }

        public async Task<string?> GetUserId(string email)
        {
            _logger.LogInformation("Getting userId by email {Email}", email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null) return user.Id;
            _logger.LogError("No userId with Email {Email} founded", email);
            return null;
        }
        
        private async Task<string> GenerateJwtToken(string email, User user)
        {
            _logger.LogInformation("Generating JWT token for user {User}", email);
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