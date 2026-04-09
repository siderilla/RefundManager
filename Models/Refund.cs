using System;
using System.Collections.Generic;

namespace RefundManager.Models;

public partial class Refund
{
    public Guid Id { get; set; }

    public Guid OutlayId { get; set; }

    public decimal Amount { get; set; }

    public bool Approved { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Outlay Outlay { get; set; } = null!;
}
