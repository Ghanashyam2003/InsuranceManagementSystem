using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
    public class ClaimInvestigation
    {
    [Key]
    public int InvestigationId { get; set; }

    [Required(ErrorMessage = "Claim Id is Required")]
    public int ClaimId { get; set; }

    [Required(ErrorMessage = "Remarks is Required")]
    [StringLength(500)]
    public string Remarks { get; set; }

    [Required(ErrorMessage = "Investigation Date is Required")]
    public DateTime InvestigationDate { get; set; }

    public int CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    [ForeignKey("ClaimId")]
    public Claim Claim { get; set; }

}
}