using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Payment
{
    public class MakePaymentDto
    {
        public int ScheduleId { get; set; }

        public string? PaymentMode { get; set; }
    }
}
