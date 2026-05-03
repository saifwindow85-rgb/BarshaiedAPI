using Domain.DTOs.ProductDTOs;
using Domain.PagedResult;
using Domain.ReadOnlyModels.Product_Models;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services_Interfaces
{
    public interface IProductService
    {
        public Task<PagedResult<ReadOnlyProductDTO>> GetAllProducts(int pageNumber, int pageSize);
        public  Task<DetailedProductDTO> GetProductById(int Id);
        public  Task<bool> Delete(int Id);
        public Task<AddUpdateServiceResponse<DetailedProductDTO>> AddProduct(AddProductDTO newProduct);
        public Task<PagedResult<ReadOnlyProductDTO>> GetExpiredProducts(int pageNumber, int pageSize);

        public Task<PagedResult<ReadOnlyProductDTO>> GetZeroQuantityProducts(int pageNumber, int pageSize);
        Task<PagedResult<ReadOnlyProductDTO>> GetProductsUnderMinQuantity(int pageNumber, int pageSize);

        Task<PagedResult<ReadOnlyProductDTO>> GetProductByNameOrBarcode(int pageNumber, int pageSize, string value);

        Task<PagedResult<ReadOnlyProductDTO>> ProductsNearingExpiry(int pageNumber, int pageSize);

        Task<AddUpdateServiceResponse<DetailedProductDTO>> UpdateProduct(int productId, UpdateProductDTO updatedProduct);
    }
}
