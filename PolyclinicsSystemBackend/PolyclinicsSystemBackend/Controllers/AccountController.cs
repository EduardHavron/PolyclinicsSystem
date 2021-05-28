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
using PolyclinicsSystemBackend.Services.Account.Interface;
using PolyclinicsSystemBackend.Services.MedicalCard.Interface.MedicalCard;

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
      return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
      var result = await _accountService.Register(model);
      return result.Succeeded ? Ok(result) : BadRequest(result);
    }
  }
}