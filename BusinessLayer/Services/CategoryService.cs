using Domain.DTOs.CategoryDTOs;
using Domain.Interfaces;
namespace BusinessLayer.Services
{
    public class CategoryService
    {
        private IcategoryRepository _repo;
        public CategoryService(IcategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<CategoryDTO>>GetCategories()
        {
            return await _repo.GetCategories();
        }
    }
}
