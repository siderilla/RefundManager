using RefundManager.Models;

namespace RefundManager.DTOs
{
    public class RefundOutputDto
    {
        public Guid Id { get; set; }

        public Guid OutlayId { get; set; }

        public decimal Amount { get; set; }

        public bool Approved { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
