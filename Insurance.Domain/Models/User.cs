using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Domain.Models
{
    public class User
    {
       
            [Key]
            public int UserId { get; set; }

            public int? CustomerId { get; set; }

            [Required(ErrorMessage = "Role Id is Required")]
            public int RoleId { get; set; }

            [Required(ErrorMessage = "First Name is Required")]
            [StringLength(100)]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is Required")]
            [StringLength(100)]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is Required")]
            [EmailAddress]
            [StringLength(150)]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is Required")]
            public string Password { get; set; }

            public bool IsEmailVerified { get; set; }

            public bool IsTwoFactorEnabled { get; set; }

            public bool IsActive { get; set; }

            public bool IsDeleted { get; set; }

            [ForeignKey("CustomerId")]
            public Customer? Customer { get; set; }

            [ForeignKey("RoleId")]
            public Role Role { get; set; }
        }
 }

