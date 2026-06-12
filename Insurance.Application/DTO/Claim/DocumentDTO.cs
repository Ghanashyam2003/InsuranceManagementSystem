using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTO.Claim
{
    public class DocumentDTO
    {
        public int DocumentId { get; set; }

        public int? ClaimId { get; set; }

        public int? CustomerId { get; set; }

        public int? PolicyId { get; set; }

        public string? EntityType { get; set; }

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public bool Status { get; set; }

        public DateTime UploadDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
