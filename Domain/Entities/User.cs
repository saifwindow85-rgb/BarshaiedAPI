using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public bool IsActive { get; set; }
        public int ?CreatedByUserId { get; set; }
        public User ? Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte Permissions { get; set; }
    }
}
