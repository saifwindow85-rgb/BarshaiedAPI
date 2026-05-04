using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public enum EnRole {Admin = 1 ,User = 2 , Viewer = 3 }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public bool IsActive { get; set; }
        public int ?CreatedByUserId { get; set; }
        public User ? Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte Permissions { get; set; }
        public ICollection<RefreshToken>RefreshTokens { get; set; } = new List<RefreshToken>();

        [NotMapped]
        public EnRole Role => GetUserRole();

        public EnRole GetUserRole()
        {
            switch(this.Permissions)
            {
                case 1:
                    {
                        return EnRole.Admin;
                    }


                case 2:
                    {
                        return EnRole.User;
                    }

                case 3:
                    {
                        return EnRole.Viewer; 
                    }

                default:
                    {
                        return EnRole.Viewer;
                    }
            }
        }
    }
}
