using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Transaction : BaseAuditableEntity
    {
        public int TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; } = null!;

        public int ?ShoppingListItemId { get; set; }
        public ShoppingListItem ShoppingListItem { get; set; }
    }
}
