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
        private ICategoryRepository _repo;
        private IValidator<AddUpdateCategoryDTO> _validator;
        public CategoryService(ICategoryRepository repo,IValidator<AddUpdateCategoryDTO> validator)
        {
            _repo = repo;
            _validator = validator;
        }
        public async Task<List<CategoryDTO>>GetCategories()
        {
            var categories = _repo.GetCategories_UnTracked();
            return await categories.Select(CategoryToDTO).ToListAsync();
        }

        public async Task<CategoryDTO>GetCategoryById(int Id)
        {
            return await _repo.GetCategories().Select(CategoryToDTO).SingleOrDefaultAsync(x=>x.Id == Id);
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
            await _repo.Add(categoryEntity);
            await _repo.SaveChanges();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<bool>Delete(int Id)
        {
            return await _repo.Delete(Id);
        }


       public async Task<AddUpdateServiceResponse<CategoryDTO>>UpdateCategory(int Id,AddUpdateCategoryDTO updatedCategory)
        {
            var validatorResult = await _validator.ValidateAsync(updatedCategory);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(validatorResult.Errors.Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = await _repo.GetCategories().SingleOrDefaultAsync(x => x.CategoryId == Id);
            if (categoryEntity == null)
            {
                return AddUpdateServiceResponse<CategoryDTO>.Failure(new List<string> { $"No Category Found With Id = {Id}" }, EnErrorTypes.NotFound);
            }
            categoryEntity.Name = updatedCategory.CategoryName;
            await _repo.SaveChanges();
            var categoryDTO = categoryEntity.ToDTO();
            return AddUpdateServiceResponse<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<List<CategoryDTO>>GetCategoryByName(string Name)
        {
            return await _repo.GetCategories().Select(CategoryToDTO).Where(x => EF.Functions.Like(x.CategoryName,$"%{Name}%")).ToListAsync();
        }
    }
}
