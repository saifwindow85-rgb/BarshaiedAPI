using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IcategoryRepository
    {
        public  IQueryable<Category> GetCategories();

        public Task<bool> Delete(int Id);
        public Task Add(Category newCategory);
        public Task SaveChanges();
    }
}
