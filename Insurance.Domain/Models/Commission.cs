using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
    public class Commission
    {
        [Key]
        public int CommissionId { get; set; }

        [Required(ErrorMessage = "Agent Id is Required")]
        public int AgentId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Commission Amount is Required")]
        public decimal CommissionAmount { get; set; }

        [ForeignKey("AgentId")]
        public Agents Agent { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}