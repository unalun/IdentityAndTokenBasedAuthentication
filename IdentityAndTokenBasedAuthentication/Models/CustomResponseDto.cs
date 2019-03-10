using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Models
{
    public class CustomResponseDto<T>
    {
        public bool Success { get; set; } = true;
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static CustomResponseDto<T> CreateSuccessResult(T data)
        {
            return new CustomResponseDto<T>
            {
                Success = true,
                Data = data,
            };
        }
    }
}
