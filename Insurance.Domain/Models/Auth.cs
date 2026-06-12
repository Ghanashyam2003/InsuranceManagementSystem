using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Auth
    {
        [Key]
        public int AuthId { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Customer? Customer { get; set; }
        public Agents? Agent { get; set; }

        public List<Notification> Notifications { get; set; } = new();
    }
}