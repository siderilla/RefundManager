namespace RefundManager.DTOs
{
    public class OutlayOutputDto
    {

        public Guid Id { get; set; }

        public string CategoryName { get; set; }
        public string EmployeeFullName { get; set; }

        public decimal Amount { get; set; }
        public bool Receipt { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }

        public string Status { get; set; } = null!;
        public decimal? RefundedAmount { get; set; }

        public decimal DailyBudget { get; set; }
        public decimal MonthlyBudget { get; set; }


    }
}
