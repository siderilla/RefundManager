using RefundManager.Models;
using System.ComponentModel.DataAnnotations;

namespace RefundManager.DTOs
{
    public class EmployeeInputDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Surname { get; set; } = null!;

        [Required, EmailAddress]
        public string Mail { get; set; } = null!;

        [Required, Phone]
        public string Phone { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Role { get; set; } = null!;

        public string? Department { get; set; } = null!;

    }
}
