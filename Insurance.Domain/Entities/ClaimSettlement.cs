using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
{
    public class ClaimSettlement
    {
        [Key]
        public int SettlementId { get; set; }

        [Required(ErrorMessage = "Claim Id is Required")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Approved Amount is Required")]
        public decimal ApprovedAmount { get; set; }

        public DateTime SettlementDate { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }
    }
}