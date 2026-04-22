using Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product :UserAbstract
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ? Barcode { get; set; }
        public int ?CategoryId { get; set; }
        public Category ?Category { get; set; } 

        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal ProfitMargin { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public DateTime ? ExpiryDate { get; set; }

        public ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();

    }
}
