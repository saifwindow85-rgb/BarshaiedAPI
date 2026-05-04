using Domain.DTOs.AuthDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public Task Add(RefreshToken refreshToken);
        public Task<UserTokenDTO> GetTokenDetails(string refreshToken);
        public Task<RefreshToken> GetRefreshTokenById(int tokenId);
    }
}
