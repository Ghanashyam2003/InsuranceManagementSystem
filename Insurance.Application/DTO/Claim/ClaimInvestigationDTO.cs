using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Claim
{
        public class ClaimInvestigationDTO
        {
            public int InvestigationId { get; set; }

            public int ClaimId { get; set; }

            public string? Remarks { get; set; }

            public DateTime InvestigationDate { get; set; }

            public int CreatedBy { get; set; }

            public int? ModifiedBy { get; set; }

            public DateTime? ModifiedAt { get; set; }
        }
    }

