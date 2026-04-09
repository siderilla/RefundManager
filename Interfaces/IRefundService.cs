using RefundManager.DTOs;
using RefundManager.Models;

namespace RefundManager.Interfaces
{
    public interface IRefundService
    {
        Task<List<RefundOutputDto>> GetAllRefundsAsync();

        Task<Refund> ProcessRefundAsync(Guid outlayId, bool isApproved, bool isAutomaticTrigger = false);

    }
}
