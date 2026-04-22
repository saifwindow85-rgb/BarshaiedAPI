using DataAccessLayer.AppDbContext;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BarshaiedDbContext _context;
        public UserRepository(BarshaiedDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserExsist(int Id)
        {
            return await _context.Users.AnyAsync(u => u.UserId == Id);
        }
    }
}
