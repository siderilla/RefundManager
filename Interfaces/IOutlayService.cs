using Microsoft.AspNetCore.Mvc;
using RefundManager.DTOs;
using RefundManager.Models;

namespace RefundManager.Interfaces
{
    public interface IOutlayService
    {

        Task<List<OutlayOutputDto>> GetOutlaysByStatus([FromQuery] string? status, Guid hrEmployeeId);
        Task<List<OutlayOutputDto>> GetAllEmployeeOutlays(Guid employeeId);
        Task<Outlay> CreateOutlayFromEmployee(Guid employeeId, OutlayInputDto outlayDto);


    }
}
