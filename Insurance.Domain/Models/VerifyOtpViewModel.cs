using System.ComponentModel.DataAnnotations;
namespace Insurance.Domain.Models
{
    public class VerifyOtpViewModel
    {
        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string OtpCode { get; set; } = "";
    }
}
