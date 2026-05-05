using BusinessLayer.Login;
using BusinessLayer.Services;
using DataAccessLayer.Configurations.Options;
using Domain.DTOs.AuthDTOs;
using Domain.Interfaces.Services_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyLoginRequest = BusinessLayer.Login.LoginRequest;
using MyRefreshRequest = Domain.DTOs.AuthDTOs.Refresh_LogOutRequest;

namespace BarshaiedAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtOption _jwt;
        public AuthController(IUserService service,IRefreshTokenService refreshTokenService,IOptions<JwtOption>jwtOptions)
        {
            _service = service;
            _refreshTokenService = refreshTokenService;
            _jwt = jwtOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] MyLoginRequest request)
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
               Encoding.UTF8.GetBytes(_jwt.Key)); // hardcoded just for test


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


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]MyRefreshRequest request)
        {
            var user = await _refreshTokenService.GetTokenDetails(request.RefreshToken);
            if (user == null)
                return Unauthorized("Invalid refresh request");

            if (user.RevokedAt != null )
                return Unauthorized("Refresh token is revoked");

            if( user.ExpiresAt<=DateTime.UtcNow)
                return Unauthorized("Refresh token is revoked"); 


            var claims = new[]
         {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),

                new Claim(ClaimTypes.Name,user.UserName), // Is This Right No Email In User Entity

                new Claim(ClaimTypes.Role,user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
           Encoding.UTF8.GetBytes(_jwt.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "StudentApi",
                audience: "StudentApiUsers",
                claims: claims,
                //expires: DateTime.UtcNow.AddMinutes(30),
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();
            await _refreshTokenService.RefreshToken(newRefreshToken, user.UserId, user.RefreshTokenId);
            return Ok(new TokenResponseDTO { AccessToken = newAccessToken ,RefreshToken = newRefreshToken});
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody]MyRefreshRequest request)
        {
            var user = await _refreshTokenService.GetTokenDetails(request.RefreshToken);
            if (user == null)
                return Ok();// we dont reveal anything to the user
            if (user.RevokedAt != null)
               return Ok(); // Until Adding Logging
            await _refreshTokenService.Logout(user.RefreshTokenId);
            return Ok("Logged out successfully");
        }
    }
}
