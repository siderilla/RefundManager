using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;

namespace RefundManager.Services
{
    public class OutlayService : IOutlayService
    {
        private readonly RefundManagerContext _context;
        private readonly IRefundService _refundService;

        public OutlayService(RefundManagerContext context, IRefundService refundService)
        {
            _context = context;
            _refundService = refundService;
        }

        public async Task<List<OutlayOutputDto>> GetOutlaysByStatus([FromQuery] string? status, Guid hrEmployeeId)
        {
            var outlaysByStatus = _context.Outlays
                .AsNoTracking()
                .Where(o => o.EmployeeId != hrEmployeeId)
                .AsQueryable();

            if (status == "In attesa")
            {
                outlaysByStatus = outlaysByStatus.Where(o => o.Refund == null);
            }
            else if (status == "Approvata")
            {
                outlaysByStatus = outlaysByStatus.Where(o => o.Refund != null && o.Refund.Approved == true);
            }
            else if (status == "Rifiutata")
            {
                outlaysByStatus = outlaysByStatus.Where(o => o.Refund != null && o.Refund.Approved == false);
            }

            var outlayList = await outlaysByStatus
                .Select(o => new OutlayOutputDto
                {
                    Id = o.Id,
                    CategoryName = o.Category.Name,
                    EmployeeFullName = o.Employee.Name + " " + o.Employee.Surname,
                    Amount = o.Amount,
                    Receipt = o.Receipt,
                    Date = o.Date,
                    Description = o.Description,
                    Status = status,
                    RefundedAmount = o.Refund.Amount,
                    MonthlyBudget = o.Category.Budget
                })
                .ToListAsync();

                return outlayList;
        }

        public async Task<List<OutlayOutputDto>> GetAllEmployeeOutlays(Guid employeeId)
        {
            var employeeOutalysList = await _context.Outlays
                .AsNoTracking()
                .Where(o => o.EmployeeId == employeeId)
                .Select(eo => new OutlayOutputDto
                {
                    Id = eo.Id,
                    CategoryName = eo.Category.Name,
                    Amount = eo.Amount,
                    Receipt = eo.Receipt,
                    Date = eo.Date,
                    Description = eo.Description,
                    Status = eo.Refund == null ? "In attesa" : (eo.Refund.Approved ? "Approvata" : "Rifiutata"),
                    RefundedAmount = eo.Refund != null ? eo.Refund.Amount : null,
                    MonthlyBudget = eo.Category.Budget
                })
                .ToListAsync();

            return employeeOutalysList;
        }


        public async Task<Outlay> CreateOutlayFromEmployee(Guid employeeId, OutlayInputDto outlayInputDto)
        {
            var outlay = new Outlay
            {
                Id = Guid.NewGuid(),
                CategoryId = outlayInputDto.CategoryId,
                EmployeeId = employeeId,
                Amount = outlayInputDto.Amount,
                Receipt = outlayInputDto.Receipt,
                Date = outlayInputDto.Date,
                Description = outlayInputDto?.Description,
            };

            _context.Outlays.Add(outlay);
            await _context.SaveChangesAsync();
            await _refundService.ProcessRefundAsync(outlay.Id, true, true);

            return outlay;
        }

    }



}
