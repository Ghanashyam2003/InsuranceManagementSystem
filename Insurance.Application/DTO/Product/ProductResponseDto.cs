namespace Insurance.Application.DTOs.Product
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductType { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }
    }
}