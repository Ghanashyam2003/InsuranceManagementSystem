using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Common.Responses
{
    public class ApiError
    {
        public string? Code { get; set; }

        public string? Details { get; set; }
    }
}
