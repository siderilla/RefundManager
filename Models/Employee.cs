using System;
using System.Collections.Generic;

namespace RefundManager.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Department { get; set; } = null!;

    public virtual ICollection<Outlay> Outlays { get; set; } = new List<Outlay>();
}
