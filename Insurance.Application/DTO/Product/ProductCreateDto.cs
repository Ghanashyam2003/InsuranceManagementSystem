using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTOs.Product
{
    public class ProductCreateDto
    {
        
        
        public string? ProductName { get; set; }

       
       
        public string? ProductType { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; }
    }
}