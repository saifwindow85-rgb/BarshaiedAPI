using Domain.DTOs.UserDTOs;
using Domain.Entities;
using Domain.PagedResult;
using Domain.ReadOnlyModels.UserDTOs;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services_Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserDTO>> GetAllUsers(int pageNumber, int pageSize);

        Task<UserDTO> GetUserById(int id);

        Task<AddUpdateServiceResponse<UserDTO>> AddUser(AddUserDTO newUser, int creatorId);

        Task<User> GetUserByUserName(string userName);

        bool VerifyPassword(string password, string passwordHash);
    }
}
