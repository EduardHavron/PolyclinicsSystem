using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.Services.Account.Interface;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizeDto model)
        {
            var result = await _accountService.Authorize(model);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _accountService.Register(model);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
    }
}