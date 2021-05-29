using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        public AdminController()
        {

        }

        [HttpGet("test")]
        public async Task<IActionResult> TestAdmin()
        {
            return Ok();
        }
    }
}