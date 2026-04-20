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
        public Task<List<LightProductObject>> GetAllProducts(int pageNumber,int pageSize);
        public Task< List<LightProductObject>> GetReadOnlyProducts(int pageNumber, int pageSize);
        public Task<DetailedProductObject> GetProdutcById(int Id);
        public Task<List<LightProductObject>> GetProductByNameOrBarcode(string Name, int pageNumber, int pageSize);
        public Task<List<LightProductObject>> GetExpiredProducts(int pageNumber, int pageSize);
        public Task<List<LightProductObject>> GetZeroQuantityProducts(int pageNumber, int pageSize);
        public Task<List<LightProductObject>> GetProductsUnderMinQuantity(int pageNumber, int pageSize);
        public Task<List<LightProductObject>> ProductsNearingExpiry(int pageNumber, int pageSize);
        public Task<Product> GetProductEntityById(int Id);
        public Task<bool> Delete(int Id);
        public Task Add(Product newProduct);
    }
}
