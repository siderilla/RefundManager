namespace RefundManager.DTOs
{
    public class OutlaySummaryOutputDto
    {
        public DateOnly Date { get; set; }
        public decimal OutlayAmount { get; set; }
        public decimal RefundAmount { get; set; }

    }
}
