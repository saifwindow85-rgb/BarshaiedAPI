using DataAccessLayer.AppDbContext;
using Domain.Entities;
using DataAccessLayer.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using Domain.PagedResult;
using DataAccessLayer.Extensions;

namespace DataAccessLayer.Repositories
{

    
    public class CategoryRespository : ICategoryRepository
    {
        private readonly BarshaiedDbContext _context;
        private Expression<Func<Category, CategoryDetailsDTO>> ToDetailsDTO = c => new CategoryDetailsDTO
        {
            Id = c.CategoryId,
            CategoryName = c.Name,
            CreatedAt = c.CreatedAt,
            CreatedByUser = c.Creator.UserName,
            UpdatedAt = c.LastUpdate,
            UpdatedByUser = c.UpdatedByUser.UserName
        };

        private Expression<Func<Category, LightCategoryDTO>> ToLightDTO = c => new LightCategoryDTO
        {
            Id = c.CategoryId,
            CategoryName = c.Name,
            CreatedAt = c.CreatedAt
        };
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

        public async Task<CategoryDetailsDTO> FindById(int Id)
        {
            return await _context.Categories.Select(ToDetailsDTO).SingleOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<PagedResult<LightCategoryDTO>> FindByName(string Name,int pageNumber,int pageSize)
        {
            var categories = await _context.Categories.AsNoTracking().Select(ToLightDTO)
                .Where(p=>EF.Functions.Like(p.CategoryName,$"%{Name}%")).ToPagedResultAsync(pageNumber, pageSize);
            return categories;
        }

     


        public async Task<PagedResult<LightCategoryDTO>> GetReadOnlyCategories(int pageNumber,int pageSize)
        {
            return await _context.Categories.AsNoTracking().Select
             (ToLightDTO).ToPagedResultAsync(pageNumber, pageSize);
        }
        public async Task<PagedResult<CategoryReportDTO>> GetCategoriesDetails(int pageNumber,int pageSize)
        {
            return await _context.Categories.AsNoTracking().Select(c => new CategoryReportDTO
            {
                Id = c.CategoryId,
                CategoryName = c.Name,
                NumberOfProducts = c.Products.Count(),
                TotalCostPrice = c.Products.Sum(p => p.CostPrice),
                TotalSellPrice = c.Products.Sum(p => p.SellPrice),
                AvrageCostPrice = c.Products.Any() ? c.Products.Average(p => p.CostPrice) : 0,
                AvrageSellPrice = c.Products.Any() ? c.Products.Average(p => p.SellPrice) : 0,
                AvrageProfitMargin = c.Products.Any() ? c.Products.Average(p => p.ProfitMargin) : 0
            }).OrderBy(c => c.Id).ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<Category> GetCategoryById(int Id)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == Id);
        }

        public async Task<bool> IsCategoryExist(int Id)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == Id);
        }
    }
}
