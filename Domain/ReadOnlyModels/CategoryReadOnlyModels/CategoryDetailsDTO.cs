using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReadOnlyModels.CategoryReadOnlyModels
{
    public class CategoryDetailsDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CreatedByUser { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string ? UpdatedByUser { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
