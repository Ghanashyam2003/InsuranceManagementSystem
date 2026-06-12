using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Claim
{
    public class ClaimSettlementCreateDTO
    {
        [Required(ErrorMessage = "Claim Id is Required")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Approved Amount is Required")]
        public decimal ApprovedAmount { get; set; }

        [Required(ErrorMessage = "Settlement Status is Required")]
        public string? SettlementStatus { get; set; }

        public int CreatedBy { get; set; }
    }
}
