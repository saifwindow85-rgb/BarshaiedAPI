using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!; // without hashing Or encrption for the current time
        public bool IsActive { get; set; }
        public int ?CreatedByUserId { get; set; }
        public User ? Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte Permissions { get; set; }
    }
}
