using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Action { get; set; } = null!;

        public string EntityName { get; set; } = null!;

        public int? EntityId { get; set; }

        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
