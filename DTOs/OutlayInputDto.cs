using System.ComponentModel.DataAnnotations;

namespace RefundManager.DTOs
{
    public class OutlayInputDto
    {
        public Guid CategoryId { get; set; }

        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        public bool Receipt { get; set; }

        public DateOnly Date { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

    }
}
