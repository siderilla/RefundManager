using System.ComponentModel.DataAnnotations;

namespace RefundManager.DTOs
{
    public class LoginInputDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Surname { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
