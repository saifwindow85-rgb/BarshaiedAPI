using DataAccessLayer.AppDbContext;
using DataAccessLayer.Extensions;
using Domain.DTOs.ShoppingListPageDTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.ShoppingListPageDTOs.ItemDTO;

namespace DataAccessLayer.Repositories
{
    public class ShoppingListPageRepository : IShoppingListPageRepository
    {
        private readonly BarshaiedDbContext _context;
   
        private Expression<Func<ShoppingListPage, ShoppingListPageReadOnlyDTO>>
            ToReadOnlyDTO = p => new ShoppingListPageReadOnlyDTO
            {
                Id = p.PageId,
                Note = p.Note,
                Items = p.ShoppingListItems.Select(i => new ItemDTO
                {
                    ProductName = i.Product.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Total = i.Total
                }).ToList(),
                Total = p.ShoppingListItems.Sum(l=>l.Total)
            };
        public ShoppingListPageRepository(BarshaiedDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ShoppingListPageReadOnlyDTO>> GetShoppingListPages(int pageNumber, int pageSize)
        {
            return await _context.ShoppingListPages.Select(ToReadOnlyDTO).ToPagedResultAsync(pageNumber, pageSize);
        }
    }
}
