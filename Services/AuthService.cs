using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RefundManager.Services
{
    public class AuthService : IAuthService
    {
        private readonly RefundManagerContext _context;
        private readonly PasswordHasher<Employee> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(RefundManagerContext context, PasswordHasher<Employee> passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<LoginOutputDto?> LoginAsync(string name, string surname, string password)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Name == name && e.Surname == surname);

            if (employee == null)
            {
                return null;
            }

            if (employee.PasswordHash == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = GenerateToken(employee);

            return new LoginOutputDto
            {
                Token = token,
                Employee = new EmployeeOutputDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    EmployeeFullName = $"{employee.Name} {employee.Surname}",
                    Mail = employee.Mail,
                    Phone = employee.Phone,
                    Role = employee.Role,
                    Department = employee.Department,
                }
            };
        }

        private string GenerateToken(Employee employee)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Role, employee.Role),
                new Claim(ClaimTypes.Name, $"{employee.Name} {employee.Surname}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
