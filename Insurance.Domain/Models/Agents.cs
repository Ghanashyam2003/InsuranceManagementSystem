using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Models
{

    public class Agents
    {
        [Key]
        public int AgentId { get; set; }

        [Required(ErrorMessage = "Agent Code is Required")]
        [StringLength(20)]
        public string AgentCode { get; set; }

        [Required(ErrorMessage = "Agent Name is Required")]
        [StringLength(150)]
        public string AgentName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [StringLength(10)]
        public string MobileNumber { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }


    }
}