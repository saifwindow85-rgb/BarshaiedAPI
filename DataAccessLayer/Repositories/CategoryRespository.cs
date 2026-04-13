using DataAccessLayer.AppDbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.DTOs.CategoryDTOs;

namespace DataAccessLayer.Repositories
{
    public  class CategoryRespository : IcategoryRepository
    {
        private BarshaiedDbContext _context;

        public CategoryRespository(BarshaiedDbContext context)
        {
            _context = context;
        }

        private Expression<Func<Category, CategoryDTO>> CategoryToDTO = c => new CategoryDTO
        {
            Id = c.CategoryId,
            CategoryName = c.Name,
            CreatedAt = c.CreatedAt,
        };
        public async Task<List<CategoryDTO>>GetCategories()
        {
            //if(!await _context.Categories.AnyAsync())
            //{
            //  await _context.Categories.AddRangeAsync(SeedMethods.SeedCategories());
            //    await _context.SaveChangesAsync();
            //}
            return await _context.Categories.AsNoTracking().Select(CategoryToDTO).ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Add(Category newCategory)
        {
            await _context.Categories.AddAsync(newCategory);
        }

        public async Task<CategoryDTO> GetCategoryById(int Id)
        {
            return await _context.Categories.Select(CategoryToDTO).SingleOrDefaultAsync(x => x.Id == Id);

        }

        public async Task<bool> Delete(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            if(category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            int rowEffected = await _context.SaveChangesAsync();
            return rowEffected > 0;
        }

        public async Task<Category> GetCategoryEntityById(int Id)
        {
            return await _context.Categories.SingleOrDefaultAsync(x=>x.CategoryId == Id);
        }
    }
}
