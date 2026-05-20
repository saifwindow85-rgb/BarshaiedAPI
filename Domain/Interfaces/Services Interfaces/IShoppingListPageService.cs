using Domain.DTOs.ShoppingListPageDTOs;
using Domain.PagedResult;
using Domain.Results;
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
        public Task<AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>> AddShoppingListPage(AddShoppingListPageDTO newPage, string? creatorId);
    }
}
