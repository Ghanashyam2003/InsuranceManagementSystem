using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class AgentCommission
    {
        [Key]
        public int AgentCommissionId { get; set; }

        [Required]
        public int AgentId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal CommissionPercentage { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        

        [ForeignKey("AgentId")]
        public Agents? Agent { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct? Product { get; set; }
    }
}