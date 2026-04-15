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

namespace BusinessLayer.Services
{
    public class ProductService
    {
        private IProductRepository _repo;
        private IValidator<AddUpdateProductDTO> _validator;
        public ProductService(IProductRepository repo,IValidator<AddUpdateProductDTO>validator)
        {
            _repo = repo;
            _validator = validator;
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

        public async Task<bool>Delete(int Id)
        {
            return await _repo.Delete(Id);
        }

        public async Task<AddUpdateServiceResponse<ProductDTO>>AddProduct(AddUpdateProductDTO newProduct)
        {
            var validatorResult = await _validator.ValidateAsync(newProduct);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<ProductDTO>.Failure(validatorResult.Errors.Select
                    (x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var productEntity = newProduct.ToEntity();

            await _repo.Add(productEntity);
            await _repo.SaveChanges();
            var productDTO =await _repo.GetAllProducts().Select(ProductToDTO).SingleOrDefaultAsync(p => p.Id == productEntity.ProductId);
            return AddUpdateServiceResponse<ProductDTO>.Success(productDTO);
        }
    }
}
