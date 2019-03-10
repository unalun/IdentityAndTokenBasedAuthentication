using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // RefreshToken And Revoked
        public string RefreshToken { get; set; }
        public bool Revoked { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
        public DateTime Issued { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
        public DateTime Expired { get; set; }
    }
}
