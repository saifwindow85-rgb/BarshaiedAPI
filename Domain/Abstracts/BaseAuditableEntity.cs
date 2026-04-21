using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstracts
{
    public abstract class BaseAuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CreatedByUserId { get; set; }
        public User Creator { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }
    }
}
