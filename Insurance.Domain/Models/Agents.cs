using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Models
{

    public class Agents
    {
        [Key]
        public int AgentId { get; set; }

        [Required(ErrorMessage = "Agent Code is Required")]
        public string AgentCode { get; set; }

        [Required(ErrorMessage = "Agent Name is Required")]
        public string AgentName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [StringLength(10)]
        public string MobileNumber { get; set; }


    }
}