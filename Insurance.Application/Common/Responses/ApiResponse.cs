using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public ApiError? Error { get; set; }

        public object? Metadata { get; set; }

        public int? TotalNumberOfRecords { get; set; }
    }
}