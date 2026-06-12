using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Claim
{
    public class DocumentCreateDTO
    {
        public int? ClaimId { get; set; }

        public int? CustomerId { get; set; }

        public int? PolicyId { get; set; }

        [Required(ErrorMessage = "Entity Type is Required")]
        public string? EntityType { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string? FileName { get; set; }

        [Required(ErrorMessage = "File Path is Required")]
        public string? FilePath { get; set; }

        public int CreatedBy { get; set; }
    }
}
