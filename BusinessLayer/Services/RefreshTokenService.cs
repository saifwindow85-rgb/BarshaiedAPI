using Domain.DTOs.AuthDTOs;
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
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task AddRefreshToken(string refreshToken,int UserId)
        {
            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(refreshToken)));
            var refrshTokenEntity = new RefreshToken
            {
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                RevokedAt = null,
               UserId = UserId
            };
            await _unitOfWork.RfershTokens.Add(refrshTokenEntity);
            await _unitOfWork.CompleteAsync();
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        public async Task<UserTokenDTO> GetTokenDetails(string userName)
        {
            return await _unitOfWork.RfershTokens.GetTokenDetails(userName);
        }

        public async Task RefreshToken(string refreshedToken,int userId, int replacedByTokenId)
        {
            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(refreshedToken)));
            var refrshTokenEntity = new RefreshToken
            {
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                RevokedAt = null,
                UserId = userId,
                ReplacedByTokenId = replacedByTokenId
            };
            await _unitOfWork.RfershTokens.Add(refrshTokenEntity);
            await _unitOfWork.CompleteAsync();
        }
    }
}
