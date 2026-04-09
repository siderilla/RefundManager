using RefundManager.Models;

namespace RefundManager.DTOs
{
    public class EmployeeInputDto
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Mail { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Role { get; set; } = null!;

    }
}
