using Domain.DTOs.UserDTOs;
using Domain.Entities;
using Domain.PagedResult;
using Domain.ReadOnlyModels.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> IsUserExsist(int ?Id);
        public Task<PagedResult<UserDTO>> GetAllUsers(int pageNumber, int pageSize);
        public Task<UserDTO>GetUserById(int Id);
        public Task<User> GetUserByUserName(string userName);
        public Task Add(User user);
    }
}
