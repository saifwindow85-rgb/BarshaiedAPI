using BusinessLayer.Login;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarshaiedAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _service;
        public AuthController(UserService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = await _service.GetUserByUserName(request.UserName);
            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!_service.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),

                new Claim(ClaimTypes.Name,user.UserName), // Is This Right No Email In User Entity

                new Claim("UserName",user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456")); // hardcoded just for test


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: "BarshiedAPI",
             audience: "BarshiedAPIUsers",
             claims: claims,
             expires: DateTime.Now.AddMinutes(30),
             signingCredentials: creds
         );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
