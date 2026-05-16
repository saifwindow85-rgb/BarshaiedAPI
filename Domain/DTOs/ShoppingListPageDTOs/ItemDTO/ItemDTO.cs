using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ShoppingListPageDTOs.ItemDTO
{
    public class ItemDTO
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total {  get; set; }
    }
}
