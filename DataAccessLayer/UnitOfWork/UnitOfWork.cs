using DataAccessLayer.AppDbContext;
using Domain.Interfaces;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BarshaiedDbContext _context;
        public ICategoryRepository Categories { get; private set; }

        public IProductRepository Products { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(BarshaiedDbContext context)
        {
            _context = context;
            Categories = new CategoryRespository(_context);
            Products = new ProductRepository(_context);
            Users = new UserRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
