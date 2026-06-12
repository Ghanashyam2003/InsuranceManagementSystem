using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTOs
{
    public class AddressDto
    {
        [Required(ErrorMessage = "Address Line 1 is required.")]
        [MaxLength(200, ErrorMessage = "Address Line 1 cannot exceed 200 characters.")]
        public string AddressLine1 { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Address Line 2 cannot exceed 200 characters.")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City name should contain only alphabets.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "State name should contain only alphabets.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "PIN Code is required.")]
        [RegularExpression(@"^[1-9]{1}[0-9]{5}$", ErrorMessage = "Invalid PIN Code. It must be a 6-digit number.")]
        public string PinCode { get; set; } = string.Empty;
    }
}