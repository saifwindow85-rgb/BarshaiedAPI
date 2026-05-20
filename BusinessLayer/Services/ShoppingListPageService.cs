using Domain.DTOs.ShoppingListPageDTOs;
using Domain.Interfaces;
using Domain.Interfaces.Services_Interfaces;
using Domain.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ShoppingListPageService : IShoppingListPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingListPageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResult<ShoppingListPageReadOnlyDTO>> GetShoppingListPage(int pageNumber, int pageSize)
        {
           return await _unitOfWork.ShoppingListPageRepository.GetShoppingListPages(pageNumber, pageSize);
        }
    }
}
