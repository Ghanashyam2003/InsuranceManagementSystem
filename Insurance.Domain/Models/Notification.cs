using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required(ErrorMessage = "User Id is Required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Message is Required")]
        [StringLength(1000)]
        public string Message { get; set; }

        [Required(ErrorMessage = "Notification Type is Required")]
        [StringLength(50)]
        public string NotificationType { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}