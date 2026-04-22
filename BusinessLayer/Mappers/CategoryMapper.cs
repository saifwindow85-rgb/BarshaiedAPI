using DataAccessLayer.Entities;
using DataAccessLayer.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
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
