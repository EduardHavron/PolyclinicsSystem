using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.Dtos.Generics;

namespace PolyclinicsSystemBackend.Services.Account.Interface
{
    public interface IAccountService
    {
        public Task<GenericResponse<IdentityError, string>> Authorize(AuthorizeDto authorizeDto);

        public Task<GenericResponse<IdentityError, string>> Register(RegisterDto registerDto);

        public Task<GenericResponse<string, string>> GetUserId(string email);
    }
}