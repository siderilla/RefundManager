using RefundManager.Models;

namespace RefundManager.DTOs
{
    public class CategoryInputDto
    {
        public decimal Budget { get; set; }
        public string Name { get; set; } = null!;

    }
}
