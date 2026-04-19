using BusinessLayer.Enums;
using BusinessLayer.Results;
using BusinessLayer.Validators;
using Domain.Entities;
using Domain.DTOs.CategoryDTOs;
using Domain.Interfaces;
using Domain.Mappers;
using FluentValidation;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        private readonly IValidator<AddUpdateCategoryDTO> _validator;
        public CategoryService( IValidator<AddUpdateCategoryDTO> validator,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<List<CategoryDTO>>GetCategories()
        {
            var categories = await _unitOfWork.categories.GetReadOnlyCategories();
            var categoriesDTO = categories.Select(CategoryToDTO.Compile()).ToList();
            return categoriesDTO;
        }

        public async Task<CategoryDTO>GetCategoryById(int Id)
        {
            var categories = await _unitOfWork.categories.GetReadOnlyCategories();
            var category = categories.Select(CategoryToDTO.Compile()).SingleOrDefault(c => c.Id == Id);
            return category;
        }

        public async Task<AddUpdateServiceResponse<CategoryDTO>>AddCategory(AddUpdateCategoryDTO newCategory)
        {
            var validatorResult = await _validator.ValidateAsync(newCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(validatorResult.Errors
                    .Select(e => $"{e.PropertyName} : {e.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = newCategory.ToEntity();
            await _unitOfWork.categories.Add(categoryEntity);
            await _unitOfWork.ComleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<bool>Delete(int Id)
        {
            return await _unitOfWork.categories.Delete(Id);
        }


       public async Task<AddUpdateServiceResponse<CategoryDTO>>UpdateCategory(int Id,AddUpdateCategoryDTO updatedCategory)
        {
            var validatorResult = await _validator.ValidateAsync(updatedCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(validatorResult.Errors.Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = await _unitOfWork.categories.FindById(Id);
            if (categoryEntity == null)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(new List<string> { $"No Category Found With Id = {Id}" }, EnErrorTypes.NotFound);
            }
            categoryEntity.Name = updatedCategory.CategoryName;
            await _unitOfWork.ComleteAsync();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<List<CategoryDTO>>GetCategoryByName(string Name)
        {
            var categories = await _unitOfWork.categories.FindByName(Name);
            var categoriesDTO = categories.Select(CategoryToDTO.Compile()).ToList();
            return categoriesDTO;
        }
    }
}
