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
using DataAccessLayer.Interfaces;
using DataAccessLayer.ReadOnlyModels.CategoryReadOnlyModels;

namespace DataAccessLayer.Repositories
{
    public class CategoryRespository : ICategoryRepository
    {
        private BarshaiedDbContext _context;

        public CategoryRespository(BarshaiedDbContext context)
        {
            _context = context;
        }


        public async Task Add(Category newCategory)
        {
            await _context.Categories.AddAsync(newCategory);
        }


        public async Task<bool> Delete(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            return true;
        }

        public async Task<Category> FindById(int Id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == Id);
            return category;
        }

        public Task<List<Category>> FindByName(string Name)
        {
            var categories = _context.Categories.Where(c => EF.Functions.Like(c.Name, $"%{Name}%")).ToListAsync();
            return categories;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }


        public async Task< List<Category>> GetReadOnlyCategories()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }
        public async Task<List<CategoryDetailsDTO>> GetCategoriesDetails()
        {
            return await _context.Categories.AsNoTracking().Select(c => new CategoryDetailsDTO
            {
                Id = c.CategoryId,
                CategoryName = c.Name,
                NumberOfProducts = c.Products.Count(),
                TotalCostPrice = c.Products.Sum(p => p.CostPrice),
                TotalSellPrice = c.Products.Sum(p => p.SellPrice),
                AvrageCostPrice = c.Products.Any() ? c.Products.Average(p => p.CostPrice) : 0,
                AvrageSellPrice = c.Products.Any() ? c.Products.Average(p => p.SellPrice) : 0,
                AvrageProfitMargin = c.Products.Any() ? c.Products.Average(p => p.ProfitMargin) : 0
            }).OrderBy(c => c.Id).ToListAsync();
        }
    }
}
