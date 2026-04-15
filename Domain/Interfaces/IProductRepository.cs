using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        public IQueryable<Product> GetAllProducts();
        public IQueryable<Product> GetProducts_UnTracked(int pageNumber);
    }
}
