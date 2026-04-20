using Domain.Entities;
using Domain.ReadOnlyModels.Product_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<ReadOnlyProductDTO>> GetAllProducts(int pageNumber,int pageSize);
        public Task< List<ReadOnlyProductDTO>> GetReadOnlyProducts(int pageNumber, int pageSize);
        public Task<DetailedProductDTO> GetProdutcById(int Id);
        public Task<List<ReadOnlyProductDTO>> GetProductByNameOrBarcode(string Name, int pageNumber, int pageSize);
        public Task<List<ReadOnlyProductDTO>> GetExpiredProducts(int pageNumber, int pageSize);
        public Task<List<ReadOnlyProductDTO>> GetZeroQuantityProducts(int pageNumber, int pageSize);
        public Task<List<ReadOnlyProductDTO>> GetProductsUnderMinQuantity(int pageNumber, int pageSize);
        public Task<List<ReadOnlyProductDTO>> ProductsNearingExpiry(int pageNumber, int pageSize);
        public Task<Product> GetProductEntityById(int Id);
        public Task<bool> Delete(int Id);
        public Task Add(Product newProduct);
    }
}
