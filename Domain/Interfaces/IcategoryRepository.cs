using Domain.Entities;
using Domain.PagedResult;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<PagedResult<LightCategoryDTO>> GetReadOnlyCategories(int pageNumber,int pageSize);
        public Task<bool> Delete(int Id);
        public Task<CategoryDetailsDTO> FindById(int Id);
        public Task<Category> GetCategoryById(int Id);
        public Task<PagedResult<LightCategoryDTO>> FindByName(string Name,int pageNumber,int pageSize);
        public Task Add(Category newCategory);
        public Task<PagedResult<CategoryReportDTO>> GetCategoriesDetails(int pageNumber,int pageSize);
        public Task<bool> IsCategoryExist(int Id);
    }
}
