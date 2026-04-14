using DataAccessLayer.AppDbContext;
using DataAccessLayer.Entities;
using Domain.Interfaces;
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

        public ProductRepository(BarshaiedDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> GetAllProducts()
        {
            return _context.Products;
        }
    }
}
