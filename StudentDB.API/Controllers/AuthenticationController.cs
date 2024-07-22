using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDB.API.ViewModels;

namespace StudentDB.API.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [Route("Login"), HttpPost]
        public async Task<IActionResult> Login(LoginVM request)
        {
            try
            {

                return Ok("testcase...");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
