using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PolyclinicsSystemBackend.Data.Entities.User;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Doctor;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.Dtos.Generics;

namespace PolyclinicsSystemBackend.Services.Account.Interface
{
    public interface IAccountService
    {
        public Task<GenericResponse<IdentityError, string>> Authorize(AuthorizeDto authorizeDto);

        public Task<GenericResponse<IdentityError, string>> Register(RegisterDto registerDto);

        public Task<GenericResponse<string, string>> GetUserId(string email);

        public Task<GenericResponse<string, List<DoctorDto>>> GetDoctors();
    }
}