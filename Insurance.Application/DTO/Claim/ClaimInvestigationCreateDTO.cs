using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTO.ClaimInvestigation
{
    public class ClaimInvestigationCreateDTO
    {
        [Required(ErrorMessage = "Claim Id is Required")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Remarks is Required")]
        public string? Remarks { get; set; }

        public int CreatedBy { get; set; }
    }
}