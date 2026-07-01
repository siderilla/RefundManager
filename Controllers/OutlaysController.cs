using Microsoft.AspNetCore.Mvc;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;

namespace RefundManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutlaysController : ControllerBase
    {
        private readonly IOutlayService _outlayService;

        public OutlaysController(IOutlayService outlayService)
        {
            _outlayService = outlayService;
        }

        // GET: api/Outlays/by-status
        [HttpGet("by-status")]
        public async Task<ActionResult<IEnumerable<OutlayOutputDto>>> GetOutlaysByStatus([FromQuery] string[]? statuses, Guid hrEmployeeId)
        {
            return await _outlayService.GetOutlaysByStatus(statuses, hrEmployeeId);
        }

        // GET: api/Outlays/by-employeeId
        [HttpGet("by-employeeId")]
        public async Task<ActionResult<IEnumerable<OutlayOutputDto>>> GetAllEmployeeOutlays(Guid employeeId)
        {
            return await _outlayService.GetAllEmployeeOutlays(employeeId);
        }

        // GET: api/Outlays/budget-summary
        [HttpGet("budget-summary")]
        public async Task<ActionResult<List<CategoryBudgetSummaryDto>>> GetBudgetSummary(
            [FromQuery] Guid employeeId,
            [FromQuery] int? month = null,
            [FromQuery] int? year = null)
        {
            var summary = await _outlayService.GetBudgetSummaryAsync(employeeId, month, year);
            return Ok(summary);
        }

        // GET: api/Outlays/calendar-summary
        [HttpGet("calendar-summary")]
        public async Task<ActionResult<List<OutlaySummaryOutputDto>>> GetCalendarSummaryAsync(
            [FromQuery] Guid? employeeId,
            [FromQuery] int month,
            [FromQuery] int year,
            [FromQuery] string? department)
        {
            var summary = await _outlayService.GetCalendarSummaryAsync(employeeId, month, year, department);
            return Ok(summary);
        }

        // POST: api/Outlays/from-employee
        [HttpPost("from-employee")]
        public async Task<ActionResult<Outlay>> PostOutlayByEmployee(Guid employeeId, [FromBody] OutlayInputDto outlay)
        {
                var newOutlay = await _outlayService.CreateOutlayFromEmployee(employeeId, outlay);

                return Ok(new
                {
                    Message = "Nuova richiesta rimborso inoltrata",
                    Outlay = newOutlay,
                });
        }
    }
}
