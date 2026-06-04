using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
    public class ClaimInvestigation
    {
        [Key]
        public int InvestigationId { get; set; }

        [Required(ErrorMessage = "Claim Id is Required")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Remarks is Required")]
        public string Remarks { get; set; }

        public DateTime InvestigationDate { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }
    }
}