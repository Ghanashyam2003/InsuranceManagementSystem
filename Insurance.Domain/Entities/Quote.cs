using InsuranceProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
{
    public class Quote
    {
        [Key]
        public int QuoteId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal PremiumAmount { get; set; }

        public DateTime QuoteDate { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerMaster Customer { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct Product { get; set; }
    }
}