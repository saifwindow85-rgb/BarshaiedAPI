using Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingListItem : BaseAuditableEntity
    {
        public int ShoppingListItemId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total {  get; set; }
        public int PageId { get; set; }
        public ShoppingListPage shoppingListPage { get; set; }
    }
}
