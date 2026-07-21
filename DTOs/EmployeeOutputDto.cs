using RefundManager.Models;

namespace RefundManager.DTOs
{
    public class EmployeeOutputDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? EmployeeFullName { get; set; }

        public string Mail { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string? Department { get; set; } = null!;

    }
}
