using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class CustomerAddress
    {
    [Key]
    public int AddressId { get; set; }

    [Required(ErrorMessage = "Customer Id is Required")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Address Line 1 is Required")]
    [StringLength(200)]
    public string? AddressLine1 { get; set; }

    [StringLength(200)]
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = "City is Required")]
    [StringLength(100)]
    public string? City { get; set; }

    [Required(ErrorMessage = "State is Required")]
    [StringLength(100)]
    public string? State { get; set; }

    [Required(ErrorMessage = "Pin Code is Required")]
    [StringLength(6)]
    public string? PinCode { get; set; }
    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

}
}