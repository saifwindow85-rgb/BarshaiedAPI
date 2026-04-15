using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.ProductDTOs
{
    public class AddUpdateProductDTO
    {

        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public int CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }

    }
}
