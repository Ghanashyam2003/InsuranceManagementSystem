using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTOs.Quote
{
    public class CreateQuoteDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal SumInsured { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public decimal BMI { get; set; }

        public bool IsDiabetic { get; set; }

        public bool HasHypertension { get; set; }

        public bool HasHeartDisease { get; set; }

        public bool IsSmoking { get; set; }

        public bool ConsumesAlcohol { get; set; }
    }
}