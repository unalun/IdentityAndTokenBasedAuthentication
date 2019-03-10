using IdentityAndTokenBasedAuthentication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }

        public ProductType ProductType { get; set; }
    }
}
