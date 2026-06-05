using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Common.Responses
{
    public static class ResponseHelper
    {
        public static ApiResponse<T> Success<T>(
            T data,
            string message,
            int? totalRecords = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Error = null,
                Metadata = new { },
                TotalNumberOfRecords = totalRecords
            };
        }

        public static ApiResponse<object> Fail(
            string message,
            string errorCode,
            string details)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null,
                Error = new ApiError
                {
                    Code = errorCode,
                    Details = details
                },
                Metadata = null,
                TotalNumberOfRecords = null
            };
        }
    }
}