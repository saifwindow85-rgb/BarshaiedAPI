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
namespace BusinessLayer.Services
{
    public class CategoryService
    {
        private Expression<Func<Category, CategoryDTO>> CategoryToDTO = c => new CategoryDTO
        {
            Id = c.CategoryId,
            CategoryName = c.Name,
            CreatedAt = c.CreatedAt,
        };
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCategoryDTO> _validator;
        private readonly IValidator<UpdateCategoryDTO> _updateValidator;
        public CategoryService( IValidator<AddCategoryDTO> validator,IUnitOfWork unitOfWork,IValidator<UpdateCategoryDTO>updateValidator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _updateValidator = updateValidator;
        }
        public async Task<List<CategoryDTO>>GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetReadOnlyCategories();
            var categoriesDTO = categories.Select(CategoryToDTO.Compile()).ToList();
            return categoriesDTO;
        }

        public async Task<CategoryDetailsDTO>GetCategoryById(int Id)
        {
            return await _unitOfWork.Categories.FindById(Id);
        }

        public async Task<AddUpdateServiceResponse<CategoryDTO>>AddCategory(AddCategoryDTO newCategory)
        {
            var validatorResult = await _validator.ValidateAsync(newCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(validatorResult.Errors
                    .Select(e => $"{e.PropertyName} : {e.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = newCategory.ToEntity();
            await _unitOfWork.Categories.Add(categoryEntity);
            await _unitOfWork.CompleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<CategoryDTO>.Success(categoryDTO);
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


       public async Task<AddUpdateServiceResponse<CategoryDTO>>UpdateCategory(int Id,UpdateCategoryDTO updatedCategory)
        {
            var validatorResult = await _updateValidator.ValidateAsync(updatedCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(validatorResult.Errors.Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = await _unitOfWork.Categories.GetCategoryById(Id);
            if (categoryEntity == null)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(new List<string> { $"No Category Found With Id = {Id}" }, EnErrorTypes.NotFound);
            }
            categoryEntity.Name = updatedCategory.CategoryName;
            categoryEntity.UpdatedByUserId = updatedCategory.UpdatedByUserId;
            categoryEntity.LastUpdate = DateTime.Now;
            await _unitOfWork.CompleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<List<CategoryDTO>>GetCategoryByName(string Name)
        {
            var categories = await _unitOfWork.Categories.FindByName(Name);
            var categoriesDTO = categories.Select(CategoryToDTO.Compile()).ToList();
            return categoriesDTO;
        }

        public async Task<List<CategoryReportDTO>>GetCategoriesDetails()
        {
            return await _unitOfWork.Categories.GetCategoriesDetails();
        }
    }
}
