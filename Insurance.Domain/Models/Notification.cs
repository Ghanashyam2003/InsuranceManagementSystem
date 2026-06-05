using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required(ErrorMessage = "User Id is Required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Message is Required")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Notification Type is Required")]
        public string NotificationType { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}