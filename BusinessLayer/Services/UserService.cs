using BusinessLayer.AddUpdateDTOs.UserDTOs;
using Domain.Interfaces;
using Domain.PagedResult;
using Domain.ReadOnlyModels.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddUserDTO> _validator;
        public UserService(IUnitOfWork unitOfWork, IValidator<AddUserDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<PagedResult<UserDTO>> GetAllUsers(int pageNumber, int pageSize)
        {
           return await  _unitOfWork.Users.GetAllUsers(pageNumber, pageSize); 
        }

        public async Task<UserDTO>GetUserById(int Id)
        {
            return await _unitOfWork.Users.GetUserById(Id);
        }
}

}

