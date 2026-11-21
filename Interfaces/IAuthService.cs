using RefundManager.DTOs;

namespace RefundManager.Interfaces
{
    public interface IAuthService
    {
        Task<LoginOutputDto?> LoginAsync(string name, string surname, string password);
    }
}
