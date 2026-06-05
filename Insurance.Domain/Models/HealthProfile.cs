using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Domain.Models
{
    internal class HealthProfile
    {
        [Key]
        public int HealthProfileId { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "BMI is Required")]
        public decimal BMI { get; set; }

        public bool IsDiabetic { get; set; }

        public bool HasHypertension { get; set; }

        public bool HasHeartDisease { get; set; }

        public bool IsSmoking { get; set; }

        public bool ConsumesAlcohol { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

    }
}
