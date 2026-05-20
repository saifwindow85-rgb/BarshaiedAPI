using BusinessLayer.Helper_Classes;
using Domain.DTOs.ShoppingListPageDTOs;
using Domain.DTOs.ShoppingListPageDTOs.ItemDTO;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Services_Interfaces;
using Domain.PagedResult;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public Func<ShoppingListItem, ItemDTO> MapToItemDTO = i => new ItemDTO
        {
            ProductName = i.Product.ProductName,
            UnitPrice = i.UnitPrice,
            Total = i.Total,
            Quantity = i.Quantity
        };
        public async Task<AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>> AddShoppingListPage(AddShoppingListPageDTO newPage, string? creatorId)
        {
            if(!HelperMethods.IsValidId(creatorId))
            {
                return AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>.InValidUserId(EnErrorTypes.InvalidAuthenticatedUserId);
            }

            var userId = int.Parse(creatorId);

            if(newPage.Items == null||!newPage.Items.Any())
            {
                return AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>.Failure(new List<string> { "At least one item is required." },
                EnErrorTypes.InvalidData);
            }

            var productsIds = newPage.Items.Select(i => i.ProductId).Distinct().ToList();
            var products =await _unitOfWork.Products.GetProductsByIds(productsIds);

            if(products.Count != productsIds.Count)
            {
                return AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>.InvalidRelatedData();
            }

            var productsDictionary = products.ToDictionary(p => p.ProductId);

            var page = new ShoppingListPage
            {
                Note = newPage.Note
            };

            foreach(var item in newPage.Items)
            {
                var product = productsDictionary[item.ProductId];
                page.ShoppingListItems.Add(new ShoppingListItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.SellPrice,
                    CreatedByUserId = userId
                });
            }
            await _unitOfWork.ShoppingListPageRepository.AddShoppingListPage(page);
            await _unitOfWork.CompleteAsync();

            var result = await _unitOfWork.ShoppingListPageRepository.GetPageById(page.PageId);

            var pageDTO = new ShoppingListPageReadOnlyDTO
            {
                Id = result.PageId,
                Items = result.ShoppingListItems.Select(MapToItemDTO).ToList(),
                Note = result.Note,
                Total = result.Total
            };
            return AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>.Success(pageDTO);
        }

        public async Task<PagedResult<ShoppingListPageReadOnlyDTO>> GetShoppingListPage(int pageNumber, int pageSize)
        {
           return await _unitOfWork.ShoppingListPageRepository.GetShoppingListPages(pageNumber, pageSize);
        }
    }
}
