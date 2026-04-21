using Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TransactionType :BaseAuditableEntity
    {
  
        public int TransactionTypeId { get; set; }
        public string TransactionName { get; set; } = null!;
    

    }
}
