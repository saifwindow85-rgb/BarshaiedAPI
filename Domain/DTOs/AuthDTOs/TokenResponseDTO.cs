using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AuthDTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken {  get; set; }
        public string RefreshToken { get; set; }
    }


    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
    }

    public class LogoutRequest
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}
