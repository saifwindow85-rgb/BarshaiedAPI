using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AuthDTOs
{
    public class UserTokenDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role {  get; set; }
        public string TokenHash { get; set; }
        public int RefreshTokenId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public int? ReplacedByTokenId { get; set; }
    }
}
