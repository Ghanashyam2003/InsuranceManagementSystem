using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Commission
{
    public class GenerateCommissionDto
    {
        public int PolicyId { get; set; }

        public decimal PremiumPaid { get; set; }
    }
}
