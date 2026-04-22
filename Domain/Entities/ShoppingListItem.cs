using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ShoppingListItem : BaseAuditableEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public bool IsPurchased { get; set; }
        
        public ICollection<Transaction>Transactions { get; set; } = new List<Transaction>();
    }
}
