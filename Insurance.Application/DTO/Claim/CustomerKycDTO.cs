using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTO.Document
{
    public class CustomerKycDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [Required]
        public string? FileName { get; set; }

        [Required]
        public string? FilePath { get; set; }

        public int CreatedBy { get; set; }
    }
}