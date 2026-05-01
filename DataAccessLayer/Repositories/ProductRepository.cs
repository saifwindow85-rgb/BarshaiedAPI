using DataAccessLayer.AppDbContext;
using DataAccessLayer.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.PagedResult;
using Domain.ReadOnlyModels.Product_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BarshaiedDbContext _context;


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
            CreatedByUser = p.Creator.UserName,
            CreatedAt = p.CreatedAt,
            ExpiryDate = p.ExpiryDate,
            UpdatedByUser = p.UpdatedByUser.UserName,
            UpdatedAt = p.LastUpdate
        };

        public ProductRepository(BarshaiedDbContext context)
        {
            _context = context;
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

        public async Task<PagedResult<ReadOnlyProductDTO>> GetReadOnlyProducts(int pageNumber,int pageSize)
        {
            return await _context.Products.AsNoTracking().Select(ToLightObject).ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<DetailedProductDTO>GetProdutcById(int Id)
        {
            return await _context.Products.Select(ToDetaieldObject).SingleOrDefaultAsync(p => p.ProductId == Id);
        }

        public async Task Add(Product product)
        {
            await _context.AddAsync(product);
        }

        public async Task<PagedResult<ReadOnlyProductDTO>> GetProductByNameOrBarcode(string nameOrBarcode,int pageNumber,int pageSize)
        {
            return await _context.Products.AsNoTracking().Select(ToLightObject).
                 Where(p => EF.Functions.Like(p.ProductName, $"%{nameOrBarcode}%") || p.Barcode == nameOrBarcode)
                 .ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<ReadOnlyProductDTO>> GetExpiredProducts(int pageNumber, int pageSize)
        {
            return await _context.Products.AsNoTracking().Select(ToLightObject)
                 .Where(p => p.ExpiryDate <= DateTime.Now.Date).ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<ReadOnlyProductDTO>> GetZeroQuantityProducts(int pageNumber, int pageSize)
        {
            return await _context.Products.AsNoTracking().Select(ToLightObject)
                .Where(p => p.Quantity <= 0).ToPagedResultAsync(pageNumber,pageSize);
        }

        public async Task<PagedResult<ReadOnlyProductDTO>> GetProductsUnderMinQuantity(int pageNumber, int pageSize)
        {
            return await _context.Products.AsNoTracking().Select(ToLightObject)
                .Where(p => p.Quantity < p.MinQuantity).ToPagedResultAsync(pageNumber, pageSize);
        }

        public  async Task<PagedResult<ReadOnlyProductDTO>> ProductsNearingExpiry(int pageNumber, int pageSize)
        {
            var toDay = DateTime.Now.Date;
            var maxDate = toDay.AddDays(10);
            return await _context.Products.Select(ToLightObject)
                .Where(p => p.ExpiryDate >= toDay && p.ExpiryDate <= maxDate).ToPagedResultAsync(pageNumber, pageSize);
        }

        public  async Task<Product> GetProductEntityById(int Id)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.ProductId == Id);
        }

        public async Task<bool> IsProductExist(int Id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == Id);
        }
    }


   
}
