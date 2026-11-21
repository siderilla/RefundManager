using RefundManager.Models;
using System.ComponentModel.DataAnnotations;

namespace RefundManager.DTOs
{
    public class CategoryInputDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal DailyBudget { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MonthlyBudget { get; set; }

    }
}
