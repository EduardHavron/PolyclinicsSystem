using System.Threading.Tasks;
using PolyclinicsSystemBackend.BAL.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;

namespace PolyclinicsSystemBackend.Services.Account.Interface
{
    public interface IAccountService
    {
        public Task<AuthorizeModel> Authorize(AuthorizeDto authorizeDto);

        public Task<AuthorizeModel> Register(RegisterDto registerDto);

        public Task<string?> GetUserId(string email);
    }
}