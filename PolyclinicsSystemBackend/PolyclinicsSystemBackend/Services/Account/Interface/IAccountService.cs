using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PolyclinicsSystemBackend.Data.Entities.User;
using PolyclinicsSystemBackend.Dtos.Account;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.AuthorizedUser;
using PolyclinicsSystemBackend.Dtos.Account.Doctor;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.Dtos.Generics;

namespace PolyclinicsSystemBackend.Services.Account.Interface
{
    public interface IAccountService
    {
        public Task<GenerisResult<IdentityError, AuthorizedUser>> Authorize(AuthorizeDto authorizeDto);

        public Task<GenerisResult<IdentityError, AuthorizedUser>> Register(RegisterDto registerDto);

        public Task<GenerisResult<string, string>> GetUserId(string email);

        public Task<GenerisResult<string, List<DoctorDto>>> GetDoctors();
    }
}