using Domain.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services_Interfaces
{
    public interface IRefreshTokenService
    {
        public Task AddRefreshToken(string refreshToken,int UserId);
        public string GenerateRefreshToken();
    }
}
