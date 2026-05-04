using DataAccessLayer.AppDbContext;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly BarshaiedDbContext _context;
        public RefreshTokenRepository(BarshaiedDbContext context)
        {
            _context = context;
        }

        public async Task Add(RefreshToken refreshToken)
        {
           await _context.AddAsync(refreshToken);
        }


    }
}
