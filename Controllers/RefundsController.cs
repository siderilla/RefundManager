using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;
using RefundManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefundManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundsController : ControllerBase
    {
        private readonly IRefundService _refundsService;

        public RefundsController(IRefundService refundsService)
        {
            _refundsService = refundsService;
        }

        // GET: api/Refunds/details
        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<RefundOutputDto>>> GetAllRefundsAsync()
        {
            return await _refundsService.GetAllRefundsAsync();
        }

    }
}
