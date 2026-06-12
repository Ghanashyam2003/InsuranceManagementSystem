using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTOs
{
    public class NomineeDto
    {
        [Required(ErrorMessage = "Nominee Name is required.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Nominee Name should contain only alphabets.")]
        public string NomineeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Relation is required.")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Relation should contain only alphabets (e.g., Brother, Spouse).")]
        public string Relation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Share Percentage is required.")]
        [Range(0.01, 100.00, ErrorMessage = "Share Percentage must be strictly between 0.01 and 100.00")]
        public decimal SharePercentage { get; set; }
    }
}