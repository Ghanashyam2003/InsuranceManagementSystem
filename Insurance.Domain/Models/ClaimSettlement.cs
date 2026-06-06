using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class ClaimSettlement
    {
        [Key]
        public int SettlementId { get; set; }

        [Required(ErrorMessage = "Claim Id is Required")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Approved Amount is Required")]
        public decimal ApprovedAmount { get; set; }

        [Required(ErrorMessage = "Settlement Status is Required")]
        [StringLength(30)]
        public string? SettlementStatus { get; set; } = "Pending";

        [Required(ErrorMessage = "Settlement Date is Required")]
        public DateTime SettlementDate { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("ClaimId")]
        public Claim? Claim { get; set; }

    }
}