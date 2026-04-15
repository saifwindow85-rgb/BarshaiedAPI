using BusinessLayer.DTOs.ProductDTOs;
using DataAccessLayer.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class ProductService
    {
        private IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        private Expression<Func<Product, ProductDTO>> ProductToDTO = p => new ProductDTO
        {
            Id = p.ProductId,
            ProductName = p.ProductName,
            CategoryName = p.Category.Name,
            Barcode = p.Barcode,
            Quantity = p.Quantity,
            ExpiryDate = p.ExpiryDate,
        };

        private Expression<Func<Product, ProductDetailsDTO>> ProductDetailsDTO = p => new ProductDetailsDTO
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Barcode = p.Barcode,
            Quantity = p.Quantity,
            MinQuantity = p.MinQuantity,
            CreatedAt = p.CreatedAt,
            CostPrice = p.CostPrice,
            SellPrice = p.SellPrice,
            ProfitMargin = p.ProfitMargin,
            ExpiryDate = p.ExpiryDate,
            UpdatedAt = p.UpdatedAt
        };

        public async Task<List<ProductDTO>>GetAllProducts(int pageNumber)
        {
            return await _repo.GetProducts_UnTracked(pageNumber).Select(ProductToDTO).ToListAsync();
        }

        public async Task<ProductDetailsDTO>GetProductById(int Id)
        {
            return await _repo.GetAllProducts().Select(ProductDetailsDTO).SingleOrDefaultAsync(p => p.ProductId == Id);
        }
    }
}
