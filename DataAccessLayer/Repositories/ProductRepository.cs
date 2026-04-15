using DataAccessLayer.AppDbContext;
using DataAccessLayer.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private BarshaiedDbContext _context;
        private int _pageSize = 10;

        public ProductRepository(BarshaiedDbContext context)
        {
            _context = context;
        }

        public Task Add(Category newCategory)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            int rowEffected = await _context.SaveChangesAsync();
            return rowEffected > 0;

        }

        public IQueryable<Product> GetAllProducts()
        {
            return _context.Products;
        }

        public IQueryable<Product> GetProducts_UnTracked(int pageNumber)
        {
            return _context.Products.Skip((pageNumber - 1) * _pageSize).Take(_pageSize).AsNoTracking();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Add(Product product)
        {
            await _context.AddAsync(product);
        }
    }
}
