using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Agent Id is Required")]
        public int AgentId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Claim Amount is Required")]
        public decimal ClaimAmount { get; set; }

        [Required(ErrorMessage = "Claim Reason is Required")]
        [StringLength(500)]
        public string ClaimReason { get; set; }

        [Required(ErrorMessage = "Claim Date is Required")]
        public DateTime ClaimDate { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        [StringLength(30)]
        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("AgentId")]
        public Agents Agent { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}