using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RefundManager.Models;

namespace RefundManager.Data;

// Keeps the public demo populated and relevant no matter when someone opens it.
// Employees and categories are seeded once and left alone; outlays/refunds are
// wiped and regenerated on every startup with dates relative to "now", so the
// dashboard's calendar and charts always have something in the current month.
public static class DemoDataSeeder
{
    private const string DemoPassword = "Demo2026!";

    public static void Seed(RefundManagerContext context, PasswordHasher<Employee> hasher)
    {
        SeedEmployees(context, hasher);
        var categories = SeedCategories(context);
        RegenerateOutlaysAndRefunds(context, categories);
    }

    private static void SeedEmployees(RefundManagerContext context, PasswordHasher<Employee> hasher)
    {
        if (!context.Employees.Any())
        {
            var employees = new[]
            {
                new Employee { Name = "Laura", Surname = "Ross", Mail = "laura.rossi@example.com", Phone = "+393332223344", Role = "Analyst" },
                new Employee { Name = "Sarah", Surname = "Collins", Mail = "sara.conti@example.com", Phone = "+393337778899", Role = "Assistant" },
                new Employee { Name = "Mark", Surname = "White", Mail = "marco.bianchi@example.com", Phone = "+393331112233", Role = "HR Manager" },
                new Employee { Name = "Andrew", Surname = "Smith", Mail = "andrea.ferrari@example.com", Phone = "+393334445566", Role = "Operations Manager" },
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

        // covers both the freshly seeded rows above and any employee a previous
        // deploy already created without a usable password.
        var withoutPassword = context.Employees.Where(e => e.PasswordHash == null).ToList();
        foreach (var employee in withoutPassword)
        {
            employee.PasswordHash = hasher.HashPassword(employee, DemoPassword);
        }
        if (withoutPassword.Count > 0)
        {
            context.SaveChanges();
        }
    }

    private static List<Category> SeedCategories(RefundManagerContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new[]
            {
                new Category { Name = "Transport", DailyBudget = 15, MonthlyBudget = 100 },
                new Category { Name = "Meals", DailyBudget = 25, MonthlyBudget = 150 },
                new Category { Name = "Accommodation", DailyBudget = 60, MonthlyBudget = 300 },
                new Category { Name = "Fuel", DailyBudget = 20, MonthlyBudget = 120 },
                new Category { Name = "Office Supplies", DailyBudget = 10, MonthlyBudget = 80 },
                new Category { Name = "Communications", DailyBudget = 8, MonthlyBudget = 50 },
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        return context.Categories.ToList();
    }

    private static readonly (string Text, string Category)[] DemoExpenses =
    {
        ("Taxi to client meeting", "Transport"),
        ("Train ticket - site visit", "Transport"),
        ("Fuel - company car", "Fuel"),
        ("Parking - trade fair", "Fuel"),
        ("Lunch with client", "Meals"),
        ("Team lunch", "Meals"),
        ("Hotel - trade fair", "Accommodation"),
        ("Hotel - two nights", "Accommodation"),
        ("Printer paper and toner", "Office Supplies"),
        ("Notebooks and pens", "Office Supplies"),
        ("Video call subscription", "Communications"),
        ("Mobile data plan top-up", "Communications"),
    };

    private static void RegenerateOutlaysAndRefunds(RefundManagerContext context, List<Category> categories)
    {
        context.Refunds.RemoveRange(context.Refunds);
        context.Outlays.RemoveRange(context.Outlays);
        context.SaveChanges();

        var employees = context.Employees.ToList();
        var categoriesByName = categories.ToDictionary(c => c.Name);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // fixed seed: same believable day-to-day pattern every restart, just
        // re-anchored to whatever "today" happens to be at that moment.
        var random = new Random(0);
        var outlays = new List<Outlay>();

        for (var dayOffset = 59; dayOffset >= 0; dayOffset--)
        {
            if (random.Next(0, 3) == 0)
            {
                continue; // not every day has an expense
            }

            var (text, categoryName) = DemoExpenses[random.Next(DemoExpenses.Length)];
            var date = today.AddDays(-dayOffset);

            outlays.Add(new Outlay
            {
                CategoryId = categoriesByName[categoryName].Id,
                EmployeeId = employees[random.Next(employees.Count)].Id,
                Amount = Math.Round((decimal)(random.NextDouble() * 80 + 5), 2),
                Receipt = random.Next(0, 4) != 0,
                Date = date,
                Description = text,
                CreatedAt = date.ToDateTime(TimeOnly.MinValue),
            });
        }

        context.Outlays.AddRange(outlays);
        context.SaveChanges();

        // outlays from the last week are left pending (no refund row yet) so
        // there's always something for the HR Manager demo account to act on.
        var refunds = new List<Refund>();
        foreach (var outlay in outlays)
        {
            var age = today.DayNumber - outlay.Date.DayNumber;
            if (age < 7 || random.Next(0, 10) == 0)
            {
                continue;
            }

            var approved = random.Next(0, 5) != 0;
            refunds.Add(new Refund
            {
                OutlayId = outlay.Id,
                Amount = approved ? outlay.Amount : 0,
                Approved = approved,
                CreatedAt = outlay.CreatedAt.AddDays(1),
            });
        }

        context.Refunds.AddRange(refunds);
        context.SaveChanges();
    }
}
