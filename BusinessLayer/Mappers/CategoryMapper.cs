using Domain.Entities;
using DataAccessLayer.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Abstracts;

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


        public static CategoryDTO ToDTO(this Category entitiy)
        {
            return new CategoryDTO
            {
                Id = entitiy.CategoryId,
                CategoryName = entitiy.Name,
                CreatedAt = entitiy.CreatedAt,
           
            };
        }
    }
}
