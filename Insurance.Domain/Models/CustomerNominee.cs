using InsuranceProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class CustomerNominee
    {
        [Key]
        public int NomineeId { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Nominee Name is Required")]
        [StringLength(100)]
        public string NomineeName { get; set; }

        [Required(ErrorMessage = "Relation is Required")]
        [StringLength(50)]
        public string Relation { get; set; }

        [Required(ErrorMessage = "Share Percentage is Required")]
        [Range(1, 100, ErrorMessage = "Share Percentage must be between 1 and 100")]
        public decimal SharePercentage { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerMaster Customer { get; set; }
    }
}