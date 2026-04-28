using Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string? Notes { get; set; }
        public bool IsPurchased { get; set; }
        
        public ICollection<Transaction>Transactions { get; set; } = new List<Transaction>();
    }
}
