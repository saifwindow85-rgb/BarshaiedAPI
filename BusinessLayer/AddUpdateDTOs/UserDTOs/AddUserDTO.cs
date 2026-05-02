using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.AddUpdateDTOs.UserDTOs
{
    public class AddUserDTO
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; }
        public byte Permissions { get; set; }
    }
}
