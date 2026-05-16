using BusinessLayer.Helper_Classes;
using BusinessLayer.Validators;
using DataAccessLayer.Mappers;
using Domain.DTOs.CategoryDTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Services_Interfaces;
using Domain.PagedResult;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using Domain.ReadOnlyModels.Product_Models;
using Domain.Results;
using FluentValidation;
using System.Linq;
using System.Linq.Expressions;
namespace BusinessLayer.Services
{
    public class CategoryService : ICategoryServices
    {
      
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCategoryDTO> _validator;
        private readonly IValidator<UpdateCategoryDTO> _updateValidator;
        public CategoryService( IValidator<AddCategoryDTO> validator,IUnitOfWork unitOfWork,IValidator<UpdateCategoryDTO>updateValidator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _updateValidator = updateValidator;
        }
        public async Task<PagedResult<LightCategoryDTO>>GetCategories(int pageNumber,int pageSize)
        {
            return await _unitOfWork.Categories.GetReadOnlyCategories(pageNumber,pageSize);
        }

        public async Task<CategoryDetailsDTO>GetCategoryById(int Id)
        {
            return await _unitOfWork.Categories.FindById(Id);
        }

        public async Task<AddUpdateServiceResponse<LightCategoryDTO>>AddCategory(AddCategoryDTO newCategory,string?creatorId)
        {
            int validUserId = -1;
            var isValidId = HelperMethods.IsValidId(creatorId);
            if (!isValidId)
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.InValidUserId(EnErrorTypes.InvalidAuthenticatedUserId);
            }
            validUserId = int.Parse(creatorId);
            var validatorResult = await _validator.ValidateAsync(newCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.Failure(validatorResult.Errors
                    .Select(e => $"{e.PropertyName} : {e.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = new Category
            {
                Name = newCategory.CategoryName,
                CreatedByUserId = validUserId,
            };
            if(!await _unitOfWork.Users.IsUserExist(categoryEntity.CreatedByUserId))
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.InvalidRelatedData();
            }
            await _unitOfWork.Categories.Add(categoryEntity);
            await _unitOfWork.CompleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<LightCategoryDTO>.Success(categoryDTO);
        }

        public async Task<bool>Delete(int Id)
        {
            if(!await _unitOfWork.Categories.Delete(Id))
            {
                return false;
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }


       public async Task<AddUpdateServiceResponse<LightCategoryDTO>>UpdateCategory(int Id,UpdateCategoryDTO updatedCategory,string?updatedByUserId)
        {
            int validUserId = -1;
            var isValidId = HelperMethods.IsValidId(updatedByUserId);
            if (!isValidId)
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.InValidUserId(EnErrorTypes.InvalidAuthenticatedUserId);
            }
            validUserId = int.Parse(updatedByUserId);
            var validatorResult = await _updateValidator.ValidateAsync(updatedCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.Failure(validatorResult.Errors.Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = await _unitOfWork.Categories.GetCategoryById(Id);
            if (categoryEntity == null)
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.Failure(new List<string> { $"No Category Found With Id = {Id}" }, EnErrorTypes.NotFound);
            }
            if(!await _unitOfWork.Users.IsUserExist(validUserId))
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.InvalidRelatedData();
            }
            categoryEntity.Name = updatedCategory.CategoryName;
            categoryEntity.UpdatedByUserId = validUserId;
            categoryEntity.LastUpdate = DateTime.Now;
            await _unitOfWork.CompleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<LightCategoryDTO>.Success(categoryDTO);
        }

        public async Task<PagedResult<LightCategoryDTO>>GetCategoryByName(string Name,int pageNumber,int pageSize)
        {
            return await _unitOfWork.Categories.FindByName(Name,pageNumber,pageSize);
        }

        public async Task<PagedResult<CategoryReportDTO>>GetCategoriesDetails(int pageNumber,int pageSize)
        {
            return await _unitOfWork.Categories.GetCategoriesDetails(pageNumber,pageSize);
        }
    }
}
