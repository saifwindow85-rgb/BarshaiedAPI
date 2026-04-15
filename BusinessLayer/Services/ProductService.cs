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

        public async Task<List<ProductDTO>>GetAllProducts(int pageNumber)
        {
            return await _repo.GetAllProducts(pageNumber).Select(ProductToDTO).ToListAsync();
        }
    }
}
