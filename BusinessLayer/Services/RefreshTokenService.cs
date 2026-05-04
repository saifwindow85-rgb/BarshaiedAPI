using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task AddRefreshToken(string refreshToken,int UserId)
        {
            var refrshTokenEntity = new RefreshToken
            {
                TokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                ExpiresAt = DateTime.Now.AddDays(7),
                RevokedAt = null,
               UserId = UserId
            };
            await _refreshTokenRepository.Add(refrshTokenEntity);
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
