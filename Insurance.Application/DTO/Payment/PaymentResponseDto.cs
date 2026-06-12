using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Payment
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }

        public int PolicyId { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentMode { get; set; }

        public string? PaymentStatus { get; set; }

        public string? TransactionNumber { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
