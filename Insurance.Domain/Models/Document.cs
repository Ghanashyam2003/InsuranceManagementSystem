using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        public int? ClaimId { get; set; }

        public int? CustomerId { get; set; }

        public int? PolicyId { get; set; }

        [Required(ErrorMessage = "Entity Type is Required")]
        [StringLength(50)]
        public string EntityType { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required(ErrorMessage = "File Path is Required")]
        public string FilePath { get; set; }

        public bool Status { get; set; }

        public DateTime UploadDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("ClaimId")]
        public Claim? Claim { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("PolicyId")]
        public Policy? Policy { get; set; }

    }
}