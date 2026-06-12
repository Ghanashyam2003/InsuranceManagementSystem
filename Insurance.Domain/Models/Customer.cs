using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CustomerCode { get; set; }

        [ForeignKey(nameof(Agent))]
        public int? AgentId { get; set; }
        public int? AuthId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First Name should contain only alphabets.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last Name should contain only alphabets.")]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? MobileNumber { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PAN Number is required.")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN Number format.")]
        public string PANNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Aadhaar Number is required.")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid Aadhaar Number format. It must be exactly 12 digits.")]
        public string AadharNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = true;

        // Navigation
        public Agents? Agent { get; set; }
        public Auth? Auth { get; set; }
        public List<CustomerAddress> Addresses { get; set; } = new();
        public List<CustomerNominee> Nominees { get; set; } = new();
        public HealthProfile? RiskProfile { get; set; }
        public List<Quote> Quotes { get; set; } = new();
        public List<Policy> Policies { get; set; } = new();
    }
}