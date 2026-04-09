using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;

namespace RefundManager.Services
{
    public class RefundService : IRefundService
    {
        private readonly RefundManagerContext _context;

        public RefundService(RefundManagerContext context)
        {
            _context = context;
        }

        public async Task<List<RefundOutputDto>> GetAllRefundsAsync()
        {
            var refundList = await _context.Refunds
                .AsNoTracking()
                .Select(r => new RefundOutputDto
                {
                    Id = r.Id,
                    OutlayId = r.OutlayId,
                    Amount = r.Amount,
                    Approved = r.Approved,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return refundList;
        }

        public async Task<Refund> ProcessRefundAsync(Guid outlayId, bool isApproved, bool isAutomaticTrigger = false)
        {
            var lastOutlay = await _context.Outlays
                .AsNoTracking()
                .Include(o => o.Category)
                .FirstOrDefaultAsync(o => o.Id == outlayId);

            var validatedOutlays = await _context.Outlays
                .AsNoTracking()
                .Where(vo => vo.EmployeeId == lastOutlay.EmployeeId
                    && vo.Date.Month == lastOutlay.Date.Month
                    && vo.Date.Year == lastOutlay.Date.Year
                    && vo.CategoryId == lastOutlay.CategoryId
                    && vo.Refund.Approved == true)
                .ToListAsync();

            var totalAmount = validatedOutlays.Sum(v => v.Amount);
            var monthlyBudget = lastOutlay.Category.Budget;
            var difference = monthlyBudget - totalAmount;

            decimal amountToRefund = 0;

            if (!isApproved)
            {
                amountToRefund = 0;
            }
            else if (difference >= lastOutlay.Amount)
            {
                amountToRefund = lastOutlay.Amount;
            }
            else if (lastOutlay.Amount > difference && difference >= 0)
            {
                amountToRefund = difference;
            }
            else
            {
                amountToRefund = 0;
            }

            if (isAutomaticTrigger && amountToRefund == 0)
            {
                return null;
            }

            var newRefund = new Refund
            {
                Id = Guid.NewGuid(),
                OutlayId = lastOutlay.Id,
                Amount = amountToRefund,
                Approved = isApproved,
                CreatedAt = DateTime.Now,
            };

            _context.Refunds.Add(newRefund);
            await _context.SaveChangesAsync();

            return newRefund;

        }

    }
}
