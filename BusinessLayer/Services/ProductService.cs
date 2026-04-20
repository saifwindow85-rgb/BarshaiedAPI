using BusinessLayer.DTOs.ProductDTOs;
using Domain.Entities;
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
using Domain.ReadOnlyModels.Product_Models;

namespace BusinessLayer.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IValidator<AddUpdateProductDTO> _validator;
        public ProductService(IProductRepository repo, IValidator<AddUpdateProductDTO> validator,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<List<LightProductObject>> GetAllProducts(int pageNumber)
        {
            return await _unitOfWork.Products.GetReadOnlyProducts(pageNumber, _pageSize);
            
        }

        public async Task<DetailedProductObject> GetProductById(int Id)
        {
            return await _unitOfWork.Products.GetProdutcById(Id);
        }

        public async Task<bool> Delete(int Id)
        {
            return await _unitOfWork.Products.Delete(Id);
        }

        public async Task<AddUpdateServiceResponse<DetailedProductObject>> AddProduct(AddUpdateProductDTO newProduct)
        {
            var validatorResult = await _validator.ValidateAsync(newProduct);
            if (!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<DetailedProductObject>.Failure(validatorResult.Errors.Select
                    (x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var productEntity = newProduct.ToEntity();

            await _unitOfWork.Products.Add(productEntity);
            await _unitOfWork.CompleteAsync();
            var productDTO = await _unitOfWork.Products.GetProdutcById(productEntity.ProductId); // To Get The CategoryNameProprty From Navagation Proprty Category
            return AddUpdateServiceResponse<DetailedProductObject>.Success(productDTO);
        }


        public async Task<List<LightProductObject>> GetExpiredProducts(int pageNumber)
        {
            return await _unitOfWork.Products.GetExpiredProducts(pageNumber, _pageSize);
        }

        public async Task<List<LightProductObject>> GetZeroQuantityProducts(int pageNumber)
        {
            return await _unitOfWork.Products.GetZeroQuantityProducts(pageNumber, _pageSize);
        }


        public async Task<List<LightProductObject>> GetProductsUnderMinQuantity(int pageNumber)
        {
            return await _unitOfWork.Products.GetProductsUnderMinQuantity(pageNumber,_pageSize);
        }

        public async Task<List<LightProductObject>>GetProductByNameOrBarcode(int pageNumber,string value)
        {
            return await _unitOfWork.Products.GetProductByNameOrBarcode(value, pageNumber, _pageSize);
        }


        public async Task<List<LightProductObject>> ProductsNearingExpiry(int pageNumber)
        {
            return await _unitOfWork.Products.ProductsNearingExpiry(pageNumber, _pageSize);
        }


        public async Task<AddUpdateServiceResponse<DetailedProductObject>>UpdateProduct(int ProductId,AddUpdateProductDTO updatedProduct)
        {
            var validatorResult = await _validator.ValidateAsync(updatedProduct);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<DetailedProductObject>.Failure(validatorResult.Errors.Select
                    (x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }

            var productEntity = await _unitOfWork.Products.GetProductEntityById(ProductId);
            if(productEntity == null)
            {
                return AddUpdateServiceResponse<DetailedProductObject>.Failure(new List<string> { $"No Product Found With Id = {ProductId}" }, EnErrorTypes.NotFound);
            }

            productEntity.ProductName = updatedProduct.ProductName;
            productEntity.Barcode = updatedProduct.Barcode;
            productEntity.Quantity = updatedProduct.Quantity;
            productEntity.CategoryId = updatedProduct.CategoryId;
            productEntity.CostPrice = updatedProduct.CostPrice;
            productEntity.SellPrice = updatedProduct.SellPrice;
            productEntity.MinQuantity = updatedProduct.MinQuantity;
            productEntity.ExpiryDate = updatedProduct.ExpiryDate;
            productEntity.UpdatedAt = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            var productDTO = await _unitOfWork.Products.GetProdutcById(ProductId);
            return AddUpdateServiceResponse<DetailedProductObject>.Success(productDTO);
        }
    }
}
