using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Entities
{
    public class CustomerMaster
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer Code is Required")]
        [StringLength(20)]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date Of Birth is Required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [StringLength(10)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "PAN Number is Required")]
        [StringLength(10)]
        public string PANNumber { get; set; }

        [Required(ErrorMessage = "Aadhar Number is Required")]
        [StringLength(12)]
        public string AadharNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}