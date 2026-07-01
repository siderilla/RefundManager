using RefundManager.Models;

namespace RefundManager.DTOs
{
    public class CategoryInputDto
    {
        public string Name { get; set; } = null!;

        public decimal DailyBudget { get; set; }
        public decimal MonthlyBudget { get; set; }

    }
}
