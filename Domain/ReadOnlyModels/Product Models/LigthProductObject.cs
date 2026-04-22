using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReadOnlyModels.Product_Models
{
    public class ReadOnlyProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public string? CategoryName { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
