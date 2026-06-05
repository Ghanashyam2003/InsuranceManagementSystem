using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class PremiumSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Installment Number is Required")]
        public int InstallmentNumber { get; set; }

        [Required(ErrorMessage = "Due Date is Required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Premium Amount is Required")]
        public decimal PremiumAmount { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("PolicyId")]
        public Policy? Policy { get; set; }
    }
}