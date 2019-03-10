using IdentityAndTokenBasedAuthentication.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Entities
{
    public class Product : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Desc { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal DiscountPrice { get; set; }

        public ProductType ProductType { get; set; }

    }
}
