using Microsoft.AspNetCore.Mvc;
using RefundManager.DTOs;
using RefundManager.Models;

namespace RefundManager.Interfaces
{
    public interface IOutlayService
    {

        Task<List<OutlayOutputDto>> GetOutlaysByStatus(string[]? statuses, Guid hrEmployeeId);
        Task<List<OutlayOutputDto>> GetAllEmployeeOutlays(Guid employeeId);
        Task<List<CategoryBudgetSummaryDto>> GetBudgetSummaryAsync(Guid employeeId, int? month = null, int? year = null);
        Task<OutlayOutputDto> CreateOutlayFromEmployee(Guid employeeId, OutlayInputDto outlayDto);
        Task<List<OutlaySummaryOutputDto>> GetCalendarSummaryAsync(Guid? employeeId, int month, int year, string? department);

    }
}
