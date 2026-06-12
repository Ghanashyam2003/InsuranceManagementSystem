using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Claim
{
    public class ClaimDTO2
    {
        [Required(ErrorMessage = "Agent Id is Required")]
        public int AgentId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Claim Amount is Required")]
        public decimal ClaimAmount { get; set; }

        [Required(ErrorMessage = "Claim Reason is Required")]
        public string? ClaimReason { get; set; }

        public int CreatedBy { get; set; }

    }
}
