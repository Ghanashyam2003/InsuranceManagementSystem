using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.SupportTicket
{
    public class CreateSupportTicketDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string? Subject { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
