using Domain.DTOs.CategoryDTOs;
using Domain.PagedResult;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services_Interfaces
{
    public interface ICategoryServices
    {
        public Task<PagedResult<LightCategoryDTO>> GetCategories(int pageNumber, int pageSize);
        public  Task<CategoryDetailsDTO> GetCategoryById(int Id);
        public Task<AddUpdateServiceResponse<LightCategoryDTO>> AddCategory(AddCategoryDTO newCategory);
        public  Task<bool> Delete(int Id);
        public Task<AddUpdateServiceResponse<LightCategoryDTO>> UpdateCategory(int Id, UpdateCategoryDTO updatedCategory);
        public  Task<PagedResult<LightCategoryDTO>> GetCategoryByName(string Name, int pageNumber, int pageSize);
        public  Task<PagedResult<CategoryReportDTO>> GetCategoriesDetails(int pageNumber, int pageSize);

    }
}
