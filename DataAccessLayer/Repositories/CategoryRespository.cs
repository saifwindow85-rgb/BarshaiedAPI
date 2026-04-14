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

namespace DataAccessLayer.Repositories
{
    public class CategoryRespository : IcategoryRepository
    {
        private BarshaiedDbContext _context;

        public CategoryRespository(BarshaiedDbContext context)
        {
            _context = context;
        }




        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
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
            int rowEffected = await _context.SaveChangesAsync();
            return rowEffected > 0;
        }


        public IQueryable<Category> GetCategories()
        {
            return _context.Categories;
        }

    }
}
