using Domain.DTOs.ShoppingListPageDTOs;
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
    }
}
