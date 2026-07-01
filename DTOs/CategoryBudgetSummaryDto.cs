namespace RefundManager.DTOs
{
    public class CategoryBudgetSummaryDto
    {
        public string CategoryName { get; set; }
        public decimal SpentToday { get; set; }
        public decimal DailyBudget { get; set; }
        public decimal SpentMonth { get; set; }
        public decimal MonthlyBudget { get; set; }
        public decimal DailyLeft => DailyBudget - SpentToday;
        public decimal MonthlyLeft => MonthlyBudget - SpentMonth;


    }
}
