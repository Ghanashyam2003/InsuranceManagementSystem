using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class UnderwritingCase
    {
        [Key]
        public int CaseId { get; set; }

        [Required(ErrorMessage = "Quote Id is Required")]
        public int QuoteId { get; set; }

        [Required(ErrorMessage = "Risk Category is Required")]
        [StringLength(50)]
        public string? RiskCategory { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("QuoteId")]
        public Quote? Quote { get; set; }
    }
}