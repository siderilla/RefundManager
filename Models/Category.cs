using System;
using System.Collections.Generic;

namespace RefundManager.Models;

public partial class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public decimal DailyBudget { get; set; }
    public decimal MonthlyBudget { get; set; }

    public virtual ICollection<Outlay> Outlays { get; set; } = new List<Outlay>();
}
