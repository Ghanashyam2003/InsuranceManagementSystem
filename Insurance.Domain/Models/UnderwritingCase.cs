using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class UnderwritingCase
    {
        [Key]
        public int CaseId { get; set; }

        [Required]
        public int QuoteId { get; set; }

        [Required]
        public string RiskCategory { get; set; }

        public string? Remarks { get; set; }

        [Required]
        public string Decision { get; set; }

        [ForeignKey("QuoteId")]
        public Quote Quote { get; set; }
    }
}