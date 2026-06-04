using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Amount is Required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Mode is Required")]
        public string PaymentMode { get; set; }

        public DateTime PaymentDate { get; set; }

        public string? TransactionNumber { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}