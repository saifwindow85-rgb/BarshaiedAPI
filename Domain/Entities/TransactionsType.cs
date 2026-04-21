using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TransactionsType
    {
  
        public int TransactionTypeId { get; set; }
        public string TransactionName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public User Creator { get; set; } = null!;

    }
}
