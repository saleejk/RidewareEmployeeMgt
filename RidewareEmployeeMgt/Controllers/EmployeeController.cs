using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RidewareEmployeeMgt.Interfaces;
using RidewareEmployeeMgt.Models;

namespace RidewareEmployeeMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly IEmployeeServices employeeService;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(IEmployeeServices _employeeServices, ILogger<EmployeeController> logger)
        {
            employeeService = _employeeServices;
            this.logger = logger;
        }

        [HttpPost("addemployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee(EmployeeDto emp)
        {
            try
            {
                var response = await employeeService.AddEmployee(emp);

                if (response != null)
                {
                    return Ok(response);
                }

                return Conflict("Employee already exists");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding the employee.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("getallemployees")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var employees = await employeeService.GetAllEmployee();

                if (employees != null && employees.Any())
                {
                    return Ok(employees);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching employees.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("getemployeebyid")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await employeeService.GetEmployeeById(id);

                if (employee != null)
                {
                    return Ok(employee);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the employee.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }


        [HttpPut("updateemployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(Employee emp)
        {
            try
            {
                var response = await employeeService.UpdateEmployee(emp);

                if (response != null)
                {
                    return Ok(response);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating the employee.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }


        [HttpDelete("deleteemployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var response = await employeeService.DeleteEmployee(id);

                if (response != null)
                {
                    return Ok(response);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the employee.");
                return StatusCode(500, "An internal server error occured.");
            }
        }

    }
}
