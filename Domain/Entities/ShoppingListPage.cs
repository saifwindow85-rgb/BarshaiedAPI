using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingListPage
    {
        public int PageId { get; set; }
        public string ?Note { get; set; }
        [NotMapped]
        public decimal Total => ShoppingListItems.Sum(x => x.Total);

        public ICollection<ShoppingListItem>ShoppingListItems { get; set; } = new List<ShoppingListItem>();
    }
}
