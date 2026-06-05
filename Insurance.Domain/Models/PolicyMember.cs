using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class PolicyMember
    {
        [Key]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Member Name is Required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Relation is Required")]
        [StringLength(50)]
        public string Relation { get; set; }

        [Required(ErrorMessage = "Date of Birth is Required")]
        public DateTime DOB { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}