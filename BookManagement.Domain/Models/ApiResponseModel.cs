using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Domain.Models
{
    public class ApiResponseModel<T>
    {
        public int Code { get; set; } = 200;       
        public string? Message { get; set; } = "SUCCESS";
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponseModel<T> Ok(T data, string? message = null)
        {
            return new ApiResponseModel<T>
            {
                Code = 200,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseModel<T> Fail(string message)
        {
            return new ApiResponseModel<T>
            {               
                Message = message
            };
        }
    }
}
