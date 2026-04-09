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
        public async Task<ActionResult<IEnumerable<OutlayOutputDto>>> GetOutlaysByStatus([FromQuery] string? status, Guid hrEmployeeId)
        {
            return await _outlayService.GetOutlaysByStatus(status, hrEmployeeId);
        }

        // GET: api/Outlays/by-employeeId
        [HttpGet("by-employeeId")]
        public async Task<ActionResult<IEnumerable<OutlayOutputDto>>> GetAllEmployeeOutlays(Guid employeeId)
        {
            return await _outlayService.GetAllEmployeeOutlays(employeeId);
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
