using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class InsuranceProduct
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is Required")]
        [StringLength(150)]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Product Type is Required")]
        [StringLength(50)]
        public string? ProductType { get; set; }

        public string? Description { get; set; }

        public String? Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}