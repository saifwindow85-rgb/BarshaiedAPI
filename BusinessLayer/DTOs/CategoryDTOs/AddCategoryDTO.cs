using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTOs.CategoryDTOs
{
    public class AddCategoryDTO
    {
        public string CategoryName { get; set; } = null!;
        public int CreatedByUserId { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string CategoryName { get; set; } = null!;
        public int UpdatedByUserId { get; set; }
    }

}
