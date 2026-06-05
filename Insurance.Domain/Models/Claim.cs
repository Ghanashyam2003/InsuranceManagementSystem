using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Claim Amount is Required")]
        public int ClaimAmount { get; set; } 

        [Required(ErrorMessage = "Claim Reason is Required")]
        public string ClaimReason { get; set; }

        [Required(ErrorMessage = "Claim Date is Required")]
        public DateTime ClaimDate { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}