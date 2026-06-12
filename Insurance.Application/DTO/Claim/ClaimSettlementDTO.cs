using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Claim
{
    public class ClaimSettlementDTO
    {
        public int SettlementId { get; set; }

        public int ClaimId { get; set; }

        public decimal ApprovedAmount { get; set; }

        public string? SettlementStatus { get; set; }

        public DateTime SettlementDate { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
