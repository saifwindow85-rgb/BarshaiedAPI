using Domain.DTOs.ProductDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers
{
    public static class ProductMapper
    {
        public static Product ToEntity(this AddProductDTO DTO)
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
                ExpiryDate = DTO.ExpiryDate,
                CreatedByUserId = DTO.CreatedByUserId,
            };
        }


    }
    } 
