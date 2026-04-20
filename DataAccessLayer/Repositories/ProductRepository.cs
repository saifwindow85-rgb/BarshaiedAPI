using Domain.AppDbContext;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ReadOnlyModels.Product_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private BarshaiedDbContext _context;


        private Expression<Func<Product, ReadOnlyProductDTO>> ToLightObject = p => new ReadOnlyProductDTO
        {
            Id = p.ProductId,
            ProductName = p.ProductName,
            Barcode = p.Barcode,
            CategoryName = p.Category.Name,
            Quantity = p.Quantity,
            MinQuantity = p.MinQuantity,
            ExpiryDate = p.ExpiryDate,

        };

        private Expression<Func<Product, DetailedProductDTO>> ToDetaieldObject = p => new DetailedProductDTO
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Barcode = p.Barcode,
            CategoryName = p.Category!.Name,
            Quantity = p.Quantity,
            MinQuantity = p.MinQuantity,
            CostPrice = p.CostPrice,
            SellPrice = p.SellPrice,
            ProfitMargin = p.ProfitMargin,
            CreatedAt = p.CreatedAt,
            ExpiryDate = p.ExpiryDate,
            UpdatedAt = p.UpdatedAt,
        };

        public ProductRepository(BarshaiedDbContext context)
        {
            _context = context;
        }

        public Task Add(Category newCategory)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            return true;

        }

        public async Task<List<ReadOnlyProductDTO>> GetAllProducts(int pageNumber,int pageSize)
        {
            return await _context.Products.Skip((pageNumber-1)*pageSize).Take(pageSize).Select(ToLightObject).ToListAsync();
        }

        public async Task<List<ReadOnlyProductDTO>> GetReadOnlyProducts(int pageNumber,int pageSize)
        {
            return await _context.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().Select(ToLightObject).ToListAsync();
        }

        public async Task<DetailedProductDTO>GetProdutcById(int Id)
        {
            return await _context.Products.Select(ToDetaieldObject).SingleOrDefaultAsync(p => p.ProductId == Id);
        }

        public async Task Add(Product product)
        {
            await _context.AddAsync(product);
        }

        public async Task<List<ReadOnlyProductDTO>> GetProductByNameOrBarcode(string nameOrBarcode,int pageNumber,int pageSize)
        {
            return await _context.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(ToLightObject)
                .Where(p => EF.Functions.Like(p.ProductName, $"%{nameOrBarcode}%") || p.Barcode == nameOrBarcode).ToListAsync();
        }

        public async Task<List<ReadOnlyProductDTO>> GetExpiredProducts(int pageNumber, int pageSize)
        {
           return await _context.Products.Skip((pageNumber - 1) * pageSize).
                Take(pageSize).Select(ToLightObject).Where(p=>p.ExpiryDate <= DateTime.UtcNow).ToListAsync();
        }

        public async Task<List<ReadOnlyProductDTO>> GetZeroQuantityProducts(int pageNumber, int pageSize)
        {
            return await _context.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(ToLightObject).Where(p => p.Quantity <= 0).ToListAsync();
        }

        public Task<List<ReadOnlyProductDTO>> GetProductsUnderMinQuantity(int pageNumber, int pageSize)
        {
            return _context.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(ToLightObject)
                .Where(p => p.Quantity < p.MinQuantity).ToListAsync();
        }

        public  async Task<List<ReadOnlyProductDTO>> ProductsNearingExpiry(int pageNumber, int pageSize)
        {
            var toDay = DateTime.Now.Date;
            var maxDate = toDay.AddDays(10);
            return await _context.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(ToLightObject).Where(p => p.ExpiryDate >= toDay && p.ExpiryDate <= maxDate).ToListAsync();
        }

        public  async Task<Product> GetProductEntityById(int Id)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.ProductId == Id);
        }
    }


   
}
