using DataAccessLayer.AppDbContext;
using Domain.DTOs.AuthDTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly BarshaiedDbContext _context;

        private Expression<Func<RefreshToken, UserTokenDTO>> ToDetaieldTokenDTO = t => new UserTokenDTO
        {
            RefreshTokenId = t.RefreshTokenId,
            TokenHash = t.TokenHash,
            ExpiresAt = t.ExpiresAt,
            RevokedAt = t.RevokedAt,
            UserId = t.UserId,
            UserName = t.User.UserName,
            Role = t.User.Role.ToString(),
            ReplacedByTokenId = t.ReplacedByTokenId,
            
        };

        public RefreshTokenRepository(BarshaiedDbContext context)
        {
            _context = context;
        }

        public async Task Add(RefreshToken refreshToken)
        {
           await _context.AddAsync(refreshToken);
        }

        public async Task<UserTokenDTO> GetTokenDetails(string userName)
        {
            return await _context.RefreshTokens.Where(t => t.User.UserName == userName).Select(ToDetaieldTokenDTO).SingleOrDefaultAsync();
        }

      
    }
}
