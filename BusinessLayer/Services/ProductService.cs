using BusinessLayer.DTOs.ProductDTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Results;
using FluentValidation;
using BusinessLayer.Enums;
using BusinessLayer.Mappers;
using BusinessLayer.Helpper_Classes;
using DataAccessLayer.ReadOnlyModels.Product_Models;

namespace BusinessLayer.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IValidator<AddProductDTO> _validator;
        private IValidator<UpdateProductDTO> _updateValidator;
        public ProductService(IProductRepository repo, IValidator<AddProductDTO> validator,IUnitOfWork unitOfWork,IValidator<UpdateProductDTO>updateValidator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _updateValidator = updateValidator;
        }
        private int _pageSize = 10;


        //private Expression<Func<Product, ProductDTO>> ProductToDTO = p => new ProductDTO
        //{
        //    Id = p.ProductId,
        //    ProductName = p.ProductName,
        //    CategoryName = p.Category.Name,
        //    Barcode = p.Barcode,
        //    Quantity = p.Quantity,
        //    MinQuantity = p.MinQuantity,
        //    ExpiryDate = p.ExpiryDate,
        //};

        //private Expression<Func<Product, ProductDetailsDTO>> ProductDetailsDTO = p => new ProductDetailsDTO
        //{
        //    ProductId = p.ProductId,
        //    ProductName = p.ProductName,
        //    Barcode = p.Barcode,
        //    Quantity = p.Quantity,
        //    MinQuantity = p.MinQuantity,
        //    CreatedAt = p.CreatedAt,
        //    CostPrice = p.CostPrice,
        //    SellPrice = p.SellPrice,
        //    ProfitMargin = p.ProfitMargin,
        //    ExpiryDate = p.ExpiryDate,
        //    UpdatedAt = p.UpdatedAt
        //};

        public async Task<List<ReadOnlyProductDTO>> GetAllProducts(int pageNumber)
        {
            return await _unitOfWork.Products.GetReadOnlyProducts(pageNumber, _pageSize);
            
        }

        public async Task<DetailedProductDTO> GetProductById(int Id)
        {
            return await _unitOfWork.Products.GetProdutcById(Id);
        }

        public async Task<bool> Delete(int Id)
        {
            if (! await _unitOfWork.Products.Delete(Id))
                return false;

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<AddUpdateServiceResponse<DetailedProductDTO>> AddProduct(AddProductDTO newProduct)
        {
            var validatorResult = await _validator.ValidateAsync(newProduct);
            if (!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<DetailedProductDTO>.Failure(validatorResult.Errors.Select
                    (x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }
            var productEntity = newProduct.ToEntity();

            await _unitOfWork.Products.Add(productEntity);
            await _unitOfWork.CompleteAsync();
            var productDTO = await _unitOfWork.Products.GetProdutcById(productEntity.ProductId); // To Get The CategoryNameProprty From Navagation Proprty Category
            return AddUpdateServiceResponse<DetailedProductDTO>.Success(productDTO);
        }


        public async Task<List<ReadOnlyProductDTO>> GetExpiredProducts(int pageNumber)
        {
            return await _unitOfWork.Products.GetExpiredProducts(pageNumber, _pageSize);
        }

        public async Task<List<ReadOnlyProductDTO>> GetZeroQuantityProducts(int pageNumber)
        {
            return await _unitOfWork.Products.GetZeroQuantityProducts(pageNumber, _pageSize);
        }


        public async Task<List<ReadOnlyProductDTO>> GetProductsUnderMinQuantity(int pageNumber)
        {
            return await _unitOfWork.Products.GetProductsUnderMinQuantity(pageNumber,_pageSize);
        }

        public async Task<List<ReadOnlyProductDTO>>GetProductByNameOrBarcode(int pageNumber,string value)
        {
            return await _unitOfWork.Products.GetProductByNameOrBarcode(value, pageNumber, _pageSize);
        }


        public async Task<List<ReadOnlyProductDTO>> ProductsNearingExpiry(int pageNumber)
        {
            return await _unitOfWork.Products.ProductsNearingExpiry(pageNumber, _pageSize);
        }


        public async Task<AddUpdateServiceResponse<DetailedProductDTO>>UpdateProduct(int ProductId,UpdateProductDTO updatedProduct)
        {
            var validatorResult = await _updateValidator.ValidateAsync(updatedProduct);
            if(!validatorResult.IsValid)
            {
                return AddUpdateServiceResponse<DetailedProductDTO>.Failure(validatorResult.Errors.Select
                    (x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), EnErrorTypes.InvalidData);
            }

            var productEntity = await _unitOfWork.Products.GetProductEntityById(ProductId);
            if(productEntity == null)
            {
                return AddUpdateServiceResponse<DetailedProductDTO>.Failure(new List<string> { $"No Product Found With Id = {ProductId}" }, EnErrorTypes.NotFound);
            }

            productEntity.ProductName = updatedProduct.ProductName;
            productEntity.Barcode = updatedProduct.Barcode;
            productEntity.Quantity = updatedProduct.Quantity;
            productEntity.CategoryId = updatedProduct.CategoryId;
            productEntity.CostPrice = updatedProduct.CostPrice;
            productEntity.SellPrice = updatedProduct.SellPrice;
            productEntity.MinQuantity = updatedProduct.MinQuantity;
            productEntity.ExpiryDate = updatedProduct.ExpiryDate;
            productEntity.UpdatedByUserId = updatedProduct.UpdatedByUserId;
            productEntity.LastUpdate = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            var productDTO = await _unitOfWork.Products.GetProdutcById(ProductId);
            return AddUpdateServiceResponse<DetailedProductDTO>.Success(productDTO);
        }
    }
}
