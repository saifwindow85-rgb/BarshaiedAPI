using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingListPage
    {
        public int PageId { get; set; }
        public string ?Note { get; set; }
        public decimal Total {  get; set; }

        public ICollection<ShoppingListItem>ShoppingListItems { get; set; } = new List<ShoppingListItem>();
    }
}
