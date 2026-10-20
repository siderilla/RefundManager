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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeOutputDto>>> GetAllEmployees()
        {
            return await _employeeService.GetAllEmployees();
        }

        // GET: api/Employee/by-name"
        [HttpGet("by-name")]
        public async Task<ActionResult<EmployeeOutputDto>> GetEmployeeByName([FromQuery] string name, [FromQuery] string surname)
        {
            var result = await _employeeService.GetEmployeeByName(name, surname);
            if (result == null) return NotFound("Dipendente non trovato");
            return Ok(result);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployeeAsync([FromBody] EmployeeInputDto employeeInputDto)
        {
            var newEmployee = await _employeeService.CreateEmployeeAsync(employeeInputDto);

            return Ok(new
            {
                Message = "Employee added",
                Employee = newEmployee,
            });
        }

    }
}
