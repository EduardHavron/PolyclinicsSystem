using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolyclinicsSystemBackend.Config.Options;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Data.Entities.User;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Doctor;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Exceptions;
using PolyclinicsSystemBackend.HelperEntities;
using PolyclinicsSystemBackend.Services.Account.Interface;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

namespace PolyclinicsSystemBackend.Services.Account.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IMedicalCardService _medicalCardService;
        private readonly IOptions<AuthOptions> _options;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AccountService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AuthOptions> options,
            ILogger<AccountService> logger,
            IMedicalCardService medicalCardService,
            IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _medicalCardService = medicalCardService ?? throw new ArgumentNullException(nameof(medicalCardService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GenericResponse<IdentityError, string>> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Email.ToUpper(),
                Email = registerDto.Email
            };

            _logger.LogInformation("Creating user with following email {User}", user.Email);

            var registerResult = await _userManager.CreateAsync(user, registerDto.Password);
            if (!registerResult.Succeeded)
            {
                _logger.LogInformation("Registration failed. Reasons: {Reasons}",
                    registerResult.Errors.Select(x => x.Description));
                return new GenericResponse<IdentityError, string> {Errors = registerResult.Errors};
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(user, registerDto.Role.ToString());
            if (assignRoleResult.Succeeded)
            {
                var result = await ProcessUserRole(registerDto);
                if (result.IsSuccess)
                {
                    return await Authorize(new AuthorizeDto {Email = registerDto.Email, Password = registerDto.Password});
                }
                return new GenericResponse<IdentityError, string>{Errors = result.Errors};
            }

            _logger.LogInformation("Adding to role failed. Reasons: {Reasons}",
                assignRoleResult.Errors.Select(x => x.Description));
            return new GenericResponse<IdentityError, string> {Errors = assignRoleResult.Errors};
        }

        public async Task<GenericResponse<IdentityError, string>> Authorize(AuthorizeDto authorizeDto)
        {
            _logger.LogInformation("Logging in user with email: {Email}", authorizeDto.Email);
            var result =
                await _signInManager.PasswordSignInAsync(authorizeDto.Email, authorizeDto.Password, false, false);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Failed to login user");
                return new GenericResponse<IdentityError, string>
                {
                    Errors = new List<IdentityError>
                    {
                        new IdentityErrorDescriber().PasswordMismatch()
                    }
                };
            }

            var appUser = _userManager.Users.AsNoTracking().SingleOrDefault(r => r.NormalizedEmail == authorizeDto.Email.ToUpper());
            var token = await GenerateJwtToken(authorizeDto.Email,
                appUser ?? throw new BusinessLogicException("Results is succeeded but user doesn't founded"));
            return new GenericResponse<IdentityError, string> {IsSuccess = true, Result = token};
        }

        public async Task<GenericResponse<string, string>> GetUserId(string email)
        {
            _logger.LogInformation("Getting userId by email {Email}", email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
                return new GenericResponse<string, string>
                {
                    IsSuccess = true,
                    Result = user.Id
                };
            _logger.LogError("No userId with Email {Email} founded", email);
            return new GenericResponse<string, string>
            {
                IsSuccess = false,
                Errors = new []{$"No userId with Email {email} founded"}
            };
        }

        public async Task<GenericResponse<string, List<DoctorDto>>> GetDoctors()
        {
            _logger.LogInformation("Retrieving all doctors");
            var doctors = await _userManager.GetUsersInRoleAsync(Roles.Doctor.ToString());
            if (doctors.Count != 0)
                return new GenericResponse<string, List<DoctorDto>>
                {
                    IsSuccess = true,
                    Result = _mapper.Map<List<DoctorDto>>(doctors)
                };
            _logger.LogError("No doctors was founded in database");
            return new GenericResponse<string, List<DoctorDto>>
            {
                IsSuccess = false,
                Errors = new[] {"No doctors was founded"}
            };
        }

        private async Task<GenericResponse<IdentityError, string>> ProcessUserRole(RegisterDto registerDto)
        {
            GenericResponse<IdentityError, string> result = registerDto.Role switch
            {
                Roles.Patient => await ProcessPatient(registerDto),
                Roles.Doctor => await ProcessDoctor(registerDto),
                _ => new GenericResponse<IdentityError, string> {IsSuccess = true}
            };

            return result;
        }

        private async Task<GenericResponse<IdentityError, string>> ProcessPatient(RegisterDto registerDto)
        {
            _logger.LogInformation("Creating a med card for patient with email {Email}", registerDto.Email);
            var userId = await GetUserId(registerDto.Email);
            if (!userId.IsSuccess || string.IsNullOrEmpty(userId.Result))
            {
                return  new GenericResponse<IdentityError, string>
                {
                    Errors = new List<IdentityError>
                    {
                        new IdentityErrorDescriber().InvalidEmail(registerDto.Email)
                    }
                };
            }
            await _medicalCardService.CreateMedicalCard(userId.Result);
            return new GenericResponse<IdentityError, string>
            {
                IsSuccess = true
            };
        }

        private async Task<GenericResponse<IdentityError, string>> ProcessDoctor(RegisterDto registerDto)
        {
            _logger.LogInformation("Assigning doctor type {DoctorType} to user {UserEmail}",
                registerDto.DoctorType, registerDto.Email);
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            user.DoctorType = registerDto.DoctorType;
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
                return new GenericResponse<IdentityError, string>
                {
                    IsSuccess = true
                };
            _logger.LogError("Updating doctor with type failed. Reasons: {Reasons}",
                updateResult.Errors.Select(x => x.Description));
            return new GenericResponse<IdentityError, string>
            {
                Errors = updateResult.Errors
            };
        }

        private async Task<string> GenerateJwtToken(string email, User user)
        {
            _logger.LogInformation("Generating JWT token for user {User}", email);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id)
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