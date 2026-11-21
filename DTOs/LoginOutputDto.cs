namespace RefundManager.DTOs
{
    public class LoginOutputDto
    {
        public string Token { get; set; } = null!;

        public EmployeeOutputDto Employee { get; set; } = null!;
    }
}
