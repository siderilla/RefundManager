using System;
using System.Collections.Generic;

namespace RefundManager.Models;

public partial class Outlay
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Guid EmployeeId { get; set; }

    public decimal Amount { get; set; }

    public bool Receipt { get; set; }

    public DateOnly Date { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Refund? Refund { get; set; }
}
