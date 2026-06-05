using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Domain.Models
{
    public class CustomerKyc
    {
        [Key]
        public int KycId { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "KYC Type is Required")]
        [StringLength(50)]
        public string? KycType { get; set; }

        [Required(ErrorMessage = "Document Number is Required")]
        [StringLength(50)]
        public string? DocumentNumber { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        [StringLength(20)]
        public string? Status { get; set; } = null;
        public int? VerifiedBy { get; set; }

        public DateTime? VerifiedDate { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("VerifiedBy")]
        public User? VerifiedUser { get; set; }
    }
}
