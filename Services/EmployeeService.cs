using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;

namespace RefundManager.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly RefundManagerContext _context;

        public EmployeeService(RefundManagerContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeOutputDto>> GetAllEmployees()
        {
            var employeeList = await _context.Employees
                .AsNoTracking()
                .Select(e => new EmployeeOutputDto
                {
                    Id = e.Id,
                    EmployeeFullName = e.Name + " " + e.Surname,
                    Mail = e.Mail,
                    Phone = e.Phone,
                    Role = e.Role
                })
                .ToListAsync();

            return employeeList;
        }

        public async Task<EmployeeOutputDto> GetEmployeeByName(string name, string surname)
        {
            var employeeByName = await _context.Employees
                .AsNoTracking()
                .Where(ns => ns.Name == name && ns.Surname == surname)
                .Select(e => new EmployeeOutputDto
                {
                    Id = e.Id,
                    Name = name,
                    Surname = surname,
                    Mail = e.Mail,
                    Phone = e.Phone,
                    Role = e.Role
                })
                .FirstOrDefaultAsync();

            return employeeByName; 

        }


        public async Task<EmployeeOutputDto> CreateEmployeeAsync(EmployeeInputDto employeeInputDto)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = employeeInputDto.Name,
                Surname = employeeInputDto.Surname,
                Mail = employeeInputDto.Mail,
                Phone = employeeInputDto.Phone,
                Role = employeeInputDto.Role
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new EmployeeOutputDto
            {
                Id = employee.Id,
                EmployeeFullName = $"{employee.Name} {employee.Surname}",
                Mail = employee.Mail,
                Phone = employee.Phone,
                Role = employee.Role
            };
        }
    }
}
