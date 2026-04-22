using Domain.Entities;
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
        public Task<List<Category>> GetCategories();
        public Task<List<Category>> GetReadOnlyCategories();
        public Task<bool> Delete(int Id);
        public Task<CategoryDetailsDTO> FindById(int Id);
        public Task<Category> GetCategoryById(int Id);
        public Task<List<Category>> FindByName(string Name);
        public Task Add(Category newCategory);
        public Task<List<CategoryReportDTO>> GetCategoriesDetails();
    }
}
