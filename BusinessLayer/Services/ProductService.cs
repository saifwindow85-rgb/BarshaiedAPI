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
using BusinessLayer.Results;
using FluentValidation;
using BusinessLayer.Enums;
using BusinessLayer.Mappers;
using BusinessLayer.Helpper_Classes;

namespace BusinessLayer.Services
{
    public class ProductService
    {
        private IProductRepository _repo;
        private IValidator<AddUpdateProductDTO> _validator;
        public ProductService(IProductRepository repo, IValidator<AddUpdateProductDTO> validator)
        {
            _repo = repo;
            _validator = validator;
        }
        private int _pageSize = 10;


        private Expression<Func<Product, ProductDTO>> ProductToDTO = p => new ProductDTO
        {
            Id = p.ProductId,
            ProductName = p.ProductName,
            CategoryName = p.Category.Name,
            Barcode = p.Barcode,
            Quantity = p.Quantity,
            MinQuantity = p.MinQuantity,
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

        public async Task<List<ProductDTO>> GetAllProducts(int pageNumber)
        {
            return await _repo.GetProducts_UnTracked().Skip((pageNumber - 1) * _pageSize).Take(_pageSize).Select(ProductToDTO).ToListAsync();
        }

        public async Task<ProductDetailsDTO> GetProductById(int Id)
        {
            return await _repo.GetAllProducts().Select(ProductDetailsDTO).SingleOrDefaultAsync(p => p.ProductId == Id);
        }

        public async Task<bool> Delete(int Id)
        {
            return await _repo.Delete(Id);
        }

        public async Task<AddUpdateServiceResponse<ProductDTO>> AddProduct(AddUpdateProductDTO newProduct)
        {
            var validatorResult = await _validator.ValidateAsync(newProduct);
            if (!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<ProductDTO>.Failure(validatorResult.Errors.Select
                    (x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var productEntity = newProduct.ToEntity();

            await _repo.Add(productEntity);
            await _repo.SaveChanges();
            var productDTO = await _repo.GetAllProducts().Select(ProductToDTO).SingleOrDefaultAsync(p => p.Id == productEntity.ProductId);
            return AddUpdateServiceResponse<ProductDTO>.Success(productDTO);
        }


        public async Task<List<ProductDTO>> GetExpiredProducts(int pageNumber)
        {
            return await _repo.GetProducts_UnTracked().Where(p => p.ExpiryDate <= DateTime.Now.Date).Skip((pageNumber - 1) * _pageSize).Take(_pageSize).Select(ProductToDTO).ToListAsync();
        }

        public async Task<List<ProductDTO>> GetZeroQuantityProducts(int pageNumber)
        {
            return await _repo.GetProducts_UnTracked().Select(ProductToDTO).Where(p => p.Quantity == 0).Skip((pageNumber - 1) * _pageSize).Take(_pageSize).ToListAsync();
        }


        public async Task<List<ProductDTO>> GetProductsUnderMinQuantity(int pageNumber)
        {
            return await _repo.GetProducts_UnTracked().Where(p => p.Quantity < p.MinQuantity)
                .Select(ProductToDTO).Skip((pageNumber - 1) * _pageSize).Take(_pageSize).ToListAsync();
        }

        public async Task<List<ProductDetailsDTO>>GetProductByNameOrBarcode(int pageNumber,string value)
        {
            return await _repo.GetProducts_UnTracked()
                 .Where(p => p.Barcode == value || p.ProductName.Contains(value)).Select(ProductDetailsDTO).ToListAsync();
        }


        public async Task<List<ProductDTO>> ProductsNearingExpiry(int pageNumber)
        {
            var toDay = DateTime.Now.Date;
            var maxDate = toDay.AddDays(10);
            return await _repo.GetProducts_UnTracked().Where
           (p => p.ExpiryDate >= toDay && p.ExpiryDate <= maxDate).Select(ProductToDTO).ToListAsync();
        }
    }
}
