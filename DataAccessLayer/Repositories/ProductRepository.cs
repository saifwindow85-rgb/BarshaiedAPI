using Domain.AppDbContext;
using Domain.Entities;
using Domain.Interfaces;
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


        private Expression<Func<Product, LightProductObject>> ToLightObject = p => new LightProductObject
        {
            Id = p.ProductId,
            ProductName = p.ProductName,
            Barcode = p.Barcode,
            CategoryName = p.Category.Name,
            Quantity = p.Quantity,
            MinQuantity = p.MinQuantity,
            ExpiryDate = p.ExpiryDate,

        };

        private Expression<Func<Product, DetailedProductObject>> ToDetaieldObject = p => new DetailedProductObject
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

        public async Task<List<DetailedProductObject>> GetAllProducts()
        {
            return await _context.Products.Select(ToLightObject).ToListAsync();
        }

        public async Task<List<LightProductObject>> GetReadOnlyProducts()
        {
            return await _context.Products.AsNoTracking().Select(ToLightObject).ToListAsync();
        }

        public async Task<DetailedProductObject>GetProdutcById(int Id)
        {
            return await _context.Products.Select(ToDetaieldObject).SingleOrDefaultAsync(p => p.ProductId == Id);
        }

        public async Task Add(Product product)
        {
            await _context.AddAsync(product);
        }
    }


    public class LightProductObject
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public string? CategoryName { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class DetailedProductObject
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal ProfitMargin { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
