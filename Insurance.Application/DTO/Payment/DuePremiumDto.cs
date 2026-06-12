using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Payment
{
    public class DuePremiumDto
    {
        public int ScheduleId { get; set; }

        public int PolicyId { get; set; }

        public int InstallmentNumber { get; set; }

        public DateTime DueDate { get; set; }

        public decimal PremiumAmount { get; set; }
    }
}
