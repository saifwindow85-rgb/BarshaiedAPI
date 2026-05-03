using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ProductDTOs
{
    public class AddProductDTO
    {

        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public int CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int CreatedByUserId { get; set; }

    }


    public class UpdateProductDTO
    {
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public int CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int UpdatedByUserId { get; set; }
    }
}
