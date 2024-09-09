using RidewareEmployeeMgt.Models;

namespace RidewareEmployeeMgt.Interfaces
{
    public interface IEmployeeServices
    {
        Task<string> AddEmployee(EmployeeDto emp);
        Task<List<Employee>> GetAllEmployee();
        Task<Employee> GetEmployeeById(int id);
        Task<string> UpdateEmployee(Employee emp);
        Task<string> DeleteEmployee(int id);
    }
}
