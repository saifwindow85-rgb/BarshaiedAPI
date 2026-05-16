using Domain.DTOs.ShoppingListPageDTOs.ItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ShoppingListPageDTOs
{
    public class ShoppingListPageReadOnlyDTO
    {
        public int Id { get; set; }
        public string ?Note { get; set; }
        public List<ItemDTO.ItemDTO>Items { get; set; }
        public decimal Total {  get; set; }
    }
}
