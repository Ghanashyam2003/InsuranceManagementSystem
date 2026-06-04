using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
{
    public class PolicyMember
    {
        [Key]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Member Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Relation is Required")]
        public string Relation { get; set; }

        [Required(ErrorMessage = "DOB is Required")]
        public DateTime DOB { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }
    }
}