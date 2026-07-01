using RefundManager.Models;

namespace RefundManager.DTOs
{
    public class CategoryOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public decimal DailyBudget { get; set; }
        public decimal MonthlyBudget { get; set; }

    }
}
