using System.ComponentModel.DataAnnotations;
namespace Insurance.Domain.Entities
{
    public class VerifyOtpViewModel
    {
        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string OtpCode { get; set; } = "";
    }
}
