using BusinessLayer.Enums;
using BusinessLayer.Results;
using BusinessLayer.Validators;
using Domain.DTOs.CategoryDTOs;
using Domain.Interfaces;
using Domain.Mappers;
using FluentValidation;
namespace BusinessLayer.Services
{
    public class CategoryService
    {
        private IcategoryRepository _repo;
        private IValidator<AddUpdateCategoryDTO> _validator;
        public CategoryService(IcategoryRepository repo,IValidator<AddUpdateCategoryDTO> validator)
        {
            _repo = repo;
            _validator = validator;
        }
        public async Task<List<CategoryDTO>>GetCategories()
        {
            return await _repo.GetCategories();
        }

        public async Task<CategoryDTO>GetCategoryById(int Id)
        {
            return await _repo.GetCategoryById(Id);
        }

        public async Task<CategoryServiceResponse<CategoryDTO>>AddCategory(AddUpdateCategoryDTO newCategory)
        {
            var validatorResult = await _validator.ValidateAsync(newCategory);
            if(!validatorResult.IsValid)
            {
                return CategoryServiceResponse<CategoryDTO>.Failure(validatorResult.Errors
                    .Select(e => $"{e.PropertyName} : {e.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = newCategory.ToEntity();
            await _repo.Add(categoryEntity);
            await _repo.SaveChanges();
            var categoryDTO = categoryEntity.ToDTO();
            return CategoryServiceResponse<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<bool>Delete(int Id)
        {
            return await _repo.Delete(Id);
        }


       public async Task<CategoryServiceResponse<CategoryDTO>>UpdateCategory(int Id,AddUpdateCategoryDTO updatedCategory)
        {
            var validatorResult = await _validator.ValidateAsync(updatedCategory);
            if(!validatorResult.IsValid)
            {
                return CategoryServiceResponse<CategoryDTO>.Failure(validatorResult.Errors.Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var categoryEntity = await _repo.GetCategoryEntityById(Id);
            if(categoryEntity == null)
            {
                return CategoryServiceResponse<CategoryDTO>.Failure(new List<string> { $"No Category Found With Id = {Id}" }, EnErrorTypes.NotFound);
            }
            categoryEntity.Name = updatedCategory.CategoryName;
            await _repo.SaveChanges();
            var categoryDTO = categoryEntity.ToDTO();
            return CategoryServiceResponse<CategoryDTO>.Success(categoryDTO);
        }
    }
}
