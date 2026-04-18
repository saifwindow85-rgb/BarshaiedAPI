using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        public  IQueryable<Category> GetCategories();
        public IQueryable<Category> GetCategories_UnTracked();
        public Task<bool> Delete(int Id);
        public Task Add(Category newCategory);
        public Task SaveChanges();
    }
}
