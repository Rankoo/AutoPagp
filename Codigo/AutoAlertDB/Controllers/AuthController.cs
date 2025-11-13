using AutoAlertDB.Models;
using AutoAlertDB.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public async Task<IActionResult> LogIn(LogIn logIn)
        {
            if (logIn == null || string.IsNullOrEmpty(logIn.Email) || string.IsNullOrEmpty(logIn.Password))
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userRepository.GetUserByEmailAsync(logIn.Email);
            if (user == null) { 
                return Unauthorized();
            }
            if (logIn.Password == user.PasswordHash)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var siginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Iusser"],
                    audience: _configuration["Jwt:Audience"],
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, logIn.Email),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString())
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: siginCredentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
