using Domain.Entities;
using Domain.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstracts;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;

namespace DataAccessLayer.Mappers
{
    public static class CategoryMapper
    {
         public static Category ToEntity(this AddCategoryDTO dto)
        {
            return new Category
            {
                Name = dto.CategoryName,
                CreatedByUserId = dto.CreatedByUserId,
            };
        }


        public static LightCategoryDTO ToDTO(this Category entitiy)
        {
            return new LightCategoryDTO
            {
                Id = entitiy.CategoryId,
                CategoryName = entitiy.Name,
                CreatedAt = entitiy.CreatedAt,
           
            };
        }
    }
}
