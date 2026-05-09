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
using Microsoft.AspNetCore.RateLimiting;


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
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService service,IRefreshTokenService refreshTokenService,IOptions<JwtOption>jwtOptions,ILogger<AuthController>logger)
        {
            _service = service;
            _refreshTokenService = refreshTokenService;
            _jwt = jwtOptions.Value;
            _logger = logger;
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Login([FromBody] MyLoginRequest request)
        {
            var user = await _service.GetUserByUserName(request.UserName);
            if (user == null)
            {
                _logger.LogWarning("Login failed. Username {Username} was not found.", request.UserName);
                return Unauthorized("Invalid credentials");
            }

            if (!_service.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed. Invalid password for username {Username}.", request.UserName);
                return Unauthorized("Invalid credentials");
            }
            if (!user.IsActive)
            {
                _logger.LogWarning("Authentication succeeded for user '{UserName}', but account status is blocked. Login prevented.", request.UserName);
                return StatusCode(403, new { Title = "Banned Account", Message = "Your Account Is Banned" });
            }

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
            _logger.LogInformation("Login succeeded for username {Username}.", request.UserName);
            return Ok(new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refrshToken,
            });
        }


        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Refresh([FromBody]MyRefreshRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var user = await _refreshTokenService.GetTokenDetails(request.RefreshToken);
            if (user == null)
            {
                _logger.LogWarning("Refresh token not found. IP: {IP}",ip);
                return Unauthorized("Invalid refresh request");
            }

            if (user.RevokedAt != null )
            {
                _logger.LogWarning( "Revoked refresh token used. IP: {IP}", ip);
                return Unauthorized("Refresh token is revoked");
            }

            if( user.ExpiresAt<=DateTime.UtcNow)
            {
                _logger.LogInformation("Expired refresh token used. IP: {IP}",ip);
                return Unauthorized("Refresh token is revoked"); 
            }


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

            _logger.LogInformation("Token refresh succeeded. UserId: {UserId}, IP: {IP}, Time: {TimeUtc}",user.UserId,ip, DateTime.UtcNow);

            return Ok(new TokenResponseDTO { AccessToken = newAccessToken ,RefreshToken = newRefreshToken});
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody]MyRefreshRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var user = await _refreshTokenService.GetTokenDetails(request.RefreshToken);
            if (user == null)
            {
                _logger.LogWarning("Logout attempt with invalid refresh token. IP: {IP}",ip);
                return Ok();// we dont reveal anything to the user
            }
            if (user.RevokedAt != null)
            {
                _logger.LogInformation("Logout called on already revoked refresh token. IP: {IP}, UserId: {UserId}",ip,user.UserId);
                return Ok(); // Until Adding Logging
            }

            await _refreshTokenService.Logout(user.RefreshTokenId);
            _logger.LogInformation("User logged out successfully. UserId: {UserId}, IP: {IP}, RefreshTokenId: {TokenId}",user.UserId,ip,user.RefreshTokenId);
            return Ok("Logged out successfully");
        }
    }
}
