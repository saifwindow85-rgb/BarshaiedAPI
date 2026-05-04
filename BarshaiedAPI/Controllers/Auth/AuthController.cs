using BusinessLayer.Login;
using BusinessLayer.Services;
using Domain.DTOs.AuthDTOs;
using Domain.Interfaces.Services_Interfaces;
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
        private readonly IUserService _service;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(IUserService service,IRefreshTokenService refreshTokenService)
        {
            _service = service;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _service.GetUserByUserName(request.UserName);
            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!_service.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");
            if (!user.IsActive)
                return StatusCode(403, new { Title = "Banned Account", Message = "Your Account Is Banned" });

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),

                new Claim(ClaimTypes.Name,user.UserName), // Is This Right No Email In User Entity

                new Claim(ClaimTypes.Role,user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456")); // hardcoded just for test


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: "BarshiedAPI",
             audience: "BarshiedAPIUsers",
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(30),
             signingCredentials: creds
         );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refrshToken = _refreshTokenService.GenerateRefreshToken();

          
            await _refreshTokenService.AddRefreshToken(refrshToken, user.UserId);

            return Ok(new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refrshToken,
            });
        }
    }
}
