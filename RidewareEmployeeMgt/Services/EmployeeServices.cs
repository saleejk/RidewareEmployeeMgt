using Microsoft.EntityFrameworkCore;
using RidewareEmployeeMgt.Data;
using RidewareEmployeeMgt.Interfaces;
using RidewareEmployeeMgt.Models;

namespace RidewareEmployeeMgt.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        public readonly DbContextClass _dbContext;
        private readonly ILogger<EmployeeServices> logger;
        public EmployeeServices(DbContextClass dbContext, ILogger<EmployeeServices> logger)
        {
            _dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<string> AddEmployee(EmployeeDto emp)
        {
            try
            {
                var isExist = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Email == emp.Email);
                if (isExist != null)
                {
                    logger.LogInformation("employee already exist");
                    return null;
                }
                var employee = new Employee { FirstName = emp.FirstName, LastName = emp.LastName, Email = emp.Email, DateOfBirth = emp.DateOfBirth, Department = emp.Department, Salary = emp.Salary };
                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();
                return "Employee Added Successfully";
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            try
            {
                var employees = await _dbContext.Employees.ToListAsync();
                if (employees != null)
                {
                    return employees;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching employees", ex);
            }
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (employee == null)
                {
                    logger.LogError("invalid employee id");
                    return null;
                }
                return employee;

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                throw new Exception("An error occured while fetching employee", ex);
            }
        }
        public async Task<string> UpdateEmployee(Employee emp)
        {
            try
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == emp.Id);
                if (employee == null)
                {
                    logger.LogInformation($"invalid employeeId {emp.Id}");
                    return null;
                }
                employee.FirstName = emp.FirstName;
                employee.LastName = emp.LastName;
                employee.Email = emp.Email;
                employee.DateOfBirth = emp.DateOfBirth;
                employee.Department = emp.Department;
                employee.Salary = emp.Salary;
                await _dbContext.SaveChangesAsync();
                return "employee updated successfully";
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<string> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (employee != null)
                {
                    _dbContext.Employees.Remove(employee);
                    await _dbContext.SaveChangesAsync();
                    return "employee deleted";
                }
                logger.LogInformation("invalid id");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null; ;
            }
        }
    }

}
