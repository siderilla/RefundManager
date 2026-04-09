using System;
using System.Collections.Generic;

namespace RefundManager.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public decimal Budget { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Outlay> Outlays { get; set; } = new List<Outlay>();
}
