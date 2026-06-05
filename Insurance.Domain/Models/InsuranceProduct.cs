using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Models
{
    public class InsuranceProduct
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is Required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Type is Required")]
        public string ProductType { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }
    }
}