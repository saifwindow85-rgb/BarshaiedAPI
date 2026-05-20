using Domain.DTOs.ShoppingListPageDTOs;
using Domain.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services_Interfaces
{
    public interface IShoppingListPageService
    {
        public Task<PagedResult<ShoppingListPageReadOnlyDTO>> GetShoppingListPage(int pageNumber, int pageSize);
    }
}
