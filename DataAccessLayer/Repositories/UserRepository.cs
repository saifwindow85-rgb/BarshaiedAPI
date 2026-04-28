using DataAccessLayer.AppDbContext;
using DataAccessLayer.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.PagedResult;
using Domain.ReadOnlyModels.UserDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private Expression<Func<User, UserDTO>> ToDTO = u => new UserDTO
        {
            Id = u.UserId,
            UserName = u.UserName,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt,
            CreatedByUser = u.Creator.UserName,
            Permissions = u.Permissions,
        };

        public async Task<PagedResult<UserDTO>> GetAllUsers(int pageNumber, int pageSize)
        {
            return await _context.Users.AsNoTracking().Select(ToDTO).ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<bool> IsUserExsist(int ?Id)
        {
            return await _context.Users.AnyAsync(u => u.UserId == Id);
        }

        public async Task<UserDTO> GetUserById(int Id)
        {
            return await _context.Users.Select(ToDTO).SingleOrDefaultAsync(u => u.Id == Id);
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
        }
    }
}
