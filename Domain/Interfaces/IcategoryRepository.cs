using DataAccessLayer.Entities;
using Domain.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IcategoryRepository
    {
        public  Task<List<CategoryDTO>> GetCategories();
        public  Task<CategoryDTO> GetCategoryById(int Id);
        public Task<bool> Delete(int Id);
        public Task Add(Category newCategory);
        public Task SaveChanges();
    }
}
