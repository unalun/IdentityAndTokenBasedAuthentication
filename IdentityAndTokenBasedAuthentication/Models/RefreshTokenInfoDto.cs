using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Models
{
    public class RefreshTokenInfoDto
    {
        public string AppUserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
