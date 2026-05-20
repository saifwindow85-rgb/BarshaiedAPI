using Domain.DTOs.ShoppingListPageDTOs.ItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ShoppingListPageDTOs
{
    public class AddShoppingListPageDTO
    {
        public string ?Note { get; set; }
        public List<AddItemDTO> Items { get; set; } = new List<AddItemDTO>(); 
    }
}
