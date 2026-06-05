using System.ComponentModel.DataAnnotations;
namespace Insurance.Domain.Models
{
    public class Verify2FAViewModel
    {
        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string Code { get; set; } = "";
    }
}
