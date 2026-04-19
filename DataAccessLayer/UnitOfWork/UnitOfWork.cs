using Domain.AppDbContext;
using Domain.Interfaces;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BarshaiedDbContext _context;
        public ICategoryRepository categories { get; private set; }

        public IProductRepository products { get; private set; }


        public UnitOfWork(BarshaiedDbContext context)
        {
            _context = context;
            categories = new CategoryRespository(_context);
            products = new ProductRepository(_context);

        }

        public async Task<int> ComleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
