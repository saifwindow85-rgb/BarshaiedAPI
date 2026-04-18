using Domain.Entities;
using Domain.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    public static class CategoryMapper
    {
         public static Category ToEntity(this AddUpdateCategoryDTO dto)
        {
            return new Category
            {
                Name = dto.CategoryName,
            };
        }


        public static CategoryDTO ToDTO(this Category entitiy)
        {
            return new CategoryDTO
            {
                Id = entitiy.CategoryId,
                CategoryName = entitiy.Name,
                CreatedAt = entitiy.CreatedAt
            };
        }
    }
}
