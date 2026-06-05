using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{ 
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

    [Required(ErrorMessage = "Commission Date is Required")]
    public DateTime CommissionDate { get; set; }

    public int CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    [ForeignKey("AgentId")]
    public Agents? Agent { get; set; }

    [ForeignKey("PolicyId")]
    public Policy? Policy { get; set; }
}
}