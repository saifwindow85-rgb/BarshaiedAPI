using Domain.DTOs.ShoppingListPageDTOs;
using Domain.Entities;
using Domain.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IShoppingListPageRepository
    {
        public Task<PagedResult<ShoppingListPageReadOnlyDTO>> GetShoppingListPages(int pageNumber, int pageSize);
        public Task AddShoppingListPage(ShoppingListPage page);
        public Task<ShoppingListPage> GetPageById(int Id);
    }
}
