using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.CategoryDTOs
{
    public class AddUpdateCategoryDTO
    {
        public string CategoryName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
