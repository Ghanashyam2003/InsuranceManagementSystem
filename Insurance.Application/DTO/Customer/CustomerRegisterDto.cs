using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTOs
{
    public class CustomerRegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First Name should contain only alphabets.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last Name should contain only alphabets.")]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "PAN Number is required.")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN Number format.")]
        public string PanNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Aadhaar Number is required.")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid Aadhaar Number format. It must be exactly 12 digits.")]
        public string AadhaarNumber { get; set; } = string.Empty;
    }
}