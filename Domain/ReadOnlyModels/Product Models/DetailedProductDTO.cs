using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ReadOnlyModels.Product_Models
{
    public class DetailedProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal ProfitMargin { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public string CreatedByUser { get; set; } = null!;
        public string UpdatedByUser { get; set; } 
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
