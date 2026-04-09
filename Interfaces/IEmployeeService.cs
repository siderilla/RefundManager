using RefundManager.DTOs;
using RefundManager.Models;

namespace RefundManager.Interfaces
{
    public interface IEmployeeService
    {

        Task<List<EmployeeOutputDto>> GetAllEmployees();
        Task<EmployeeOutputDto> GetEmployeeByName(string name, string surname);
        Task<Employee> CreateEmployeeAsync(EmployeeInputDto employeeInputDto);

    }
}
