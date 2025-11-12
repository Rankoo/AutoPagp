using AutoAlertDB.Models;
using AutoAlertDB.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoAlertDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        { 
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("logIn")]
        public async Task<ActionResult> LogIn(LogIn logIn)
        {

            return Ok();
        }
    }
}
