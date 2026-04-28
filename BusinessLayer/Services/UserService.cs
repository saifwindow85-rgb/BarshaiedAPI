using BCrypt.Net;
using BusinessLayer.AddUpdateDTOs.UserDTOs;
using BusinessLayer.Enums;
using BusinessLayer.Results;
using Domain.Entities;
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
            return await _unitOfWork.Users.GetAllUsers(pageNumber, pageSize);
        }

        public async Task<UserDTO> GetUserById(int Id)
        {
            return await _unitOfWork.Users.GetUserById(Id);
        }


        public async Task<AddUpdateServiceResponse<UserDTO>> AddUser(AddUserDTO newUser)
        {
            var validatorResult = await _validator.ValidateAsync(newUser);
            if (!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<UserDTO>.Failure(validatorResult.Errors.
                    Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            if (!await _unitOfWork.Users.IsUserExsist(newUser.CreatedByUserId))
            {
                return AddUpdateServiceResponse<UserDTO>.InvalidRelatedData();
            }
            var userEntity = new User
            {
                UserName = newUser.UserName,
                CreatedByUserId = newUser.CreatedByUserId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
                Permissions = newUser.Permissions,
                IsActive = newUser.IsActive,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.Users.Add(userEntity);
            await _unitOfWork.CompleteAsync();
            var userDTO = await _unitOfWork.Users.GetUserById(userEntity.UserId);
            return AddUpdateServiceResponse<UserDTO>.Success(userDTO);
        }

    }
}

