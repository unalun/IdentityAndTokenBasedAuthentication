using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityAndTokenBasedAuthentication.Entities;
using IdentityAndTokenBasedAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAndTokenBasedAuthentication.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CustomIdentityDbContext _Context;
        private readonly List<ProductDto> ProductList;
        public ProductController(CustomIdentityDbContext context)
        {
            _Context = context;
            ProductList = new List<ProductDto>();
        }

        [HttpGet("GetProductList")]
        public async Task<IActionResult> GetProductList()
        {
            var GetProductList = await _Context.Product.ToListAsync();
            foreach (Product product in GetProductList)
            {
                ProductList.Add(new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Desc = product.Desc,
                    DiscountPrice = product.DiscountPrice,
                    ProductType = product.ProductType
                });
            }

            var data = CustomResponseDto<List<ProductDto>>.CreateSuccessResult(ProductList);

            return Ok(data);
        }
    }
}