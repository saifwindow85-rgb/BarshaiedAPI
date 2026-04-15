using BusinessLayer.DTOs.ProductDTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers
{
    public static class ProductMapper
    {
        public static Product ToEntity(this AddUpdateProductDTO DTO)
        {
            return new Product
            {
                ProductName = DTO.ProductName,
                CategoryId = DTO.CategoryId,
                Barcode = DTO.Barcode,
                Quantity = DTO.Quantity,
                MinQuantity = DTO.MinQuantity,
                SellPrice = DTO.SellPrice,
                CostPrice = DTO.CostPrice,
            };
        }

        public static ProductDTO ToDTO(this Product entity)
        {
            return new ProductDTO
            {
                Id = entity.ProductId,
                ProductName = entity.ProductName,
                Barcode = entity.Barcode,
                Quantity = entity.Quantity,
                ExpiryDate = entity.ExpiryDate,
                CategoryName = entity.Category.Name,

            };
    }
    } }
