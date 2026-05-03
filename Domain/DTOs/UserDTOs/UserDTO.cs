using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReadOnlyModels.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string CreatedByUser { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte Permissions { get; set; }
    }
}
