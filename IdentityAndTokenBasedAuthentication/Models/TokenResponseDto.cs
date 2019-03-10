using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Models
{
    public class TokenResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string AppUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
