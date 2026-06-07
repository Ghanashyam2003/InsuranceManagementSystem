using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Domain.Models
{
    public class SupportTicket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [StringLength(50)]
        public string? TicketNumber { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Subject { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(30)]
        public string? Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
