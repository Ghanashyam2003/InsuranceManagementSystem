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

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}