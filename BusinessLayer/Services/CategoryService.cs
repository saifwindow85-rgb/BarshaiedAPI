using BusinessLayer.Enums;
using BusinessLayer.Results;
using BusinessLayer.Validators;
using Domain.Entities;
using DataAccessLayer.DTOs.CategoryDTOs;
using Domain.Interfaces;
using DataAccessLayer.Mappers;
using FluentValidation;
using System.Linq.Expressions;
using System.Linq;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using Domain.PagedResult;
namespace BusinessLayer.Services
{
    public class CategoryService
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

        public async Task<AddUpdateServiceResponse<LightCategoryDTO>>AddCategory(AddCategoryDTO newCategory)
        {
            var validatorResult = await _validator.ValidateAsync(newCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.Failure(validatorResult.Errors
                    .Select(e => $"{e.PropertyName} : {e.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = newCategory.ToEntity();
            if(!await _unitOfWork.Users.IsUserExsist(categoryEntity.CreatedByUserId))
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


       public async Task<AddUpdateServiceResponse<LightCategoryDTO>>UpdateCategory(int Id,UpdateCategoryDTO updatedCategory)
        {
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
            if(!await _unitOfWork.Users.IsUserExsist(updatedCategory.UpdatedByUserId))
            {
                return AddUpdateServiceResponse<LightCategoryDTO>.InvalidRelatedData();
            }
            categoryEntity.Name = updatedCategory.CategoryName;
            categoryEntity.UpdatedByUserId = updatedCategory.UpdatedByUserId;
            categoryEntity.LastUpdate = DateTime.Now;
            await _unitOfWork.CompleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<LightCategoryDTO>.Success(categoryDTO);
        }

        public async Task<PagedResult<LightCategoryDTO>>GetCategoryByName(string Name,int pageNumber,int pageSize)
        {
            return await _unitOfWork.Categories.FindByName(Name,pageNumber,pageSize);
        }

        public async Task<List<CategoryReportDTO>>GetCategoriesDetails()
        {
            return await _unitOfWork.Categories.GetCategoriesDetails();
        }
    }
}
