namespace RefundManager.DTOs
{
    public class OutlayInputDto
    {
        public Guid CategoryId { get; set; }

        public decimal Amount { get; set; }

        public bool Receipt { get; set; }

        public DateOnly Date { get; set; }

        public string? Description { get; set; }

    }
}
