using EHD.BAL.Interface;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static EHD.BAL.Domain_Models.DepartmentDTO;
using static EHD.BAL.Exceptions.AllExceptions;

namespace EHD.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomExceptionFilter]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user) 
        {
            _user = user;
        }

        [HttpPost]
        public async Task AddOrUpdateEmployee([FromBody] Employee employee)
        {
            await _user.AddOrUpdateEmployee(employee);
        }

        [HttpGet]
        public async Task<ActionResult<Employee>> GetUserByEmployeeId(string employeeId)
        {
            var employee = await _user.GetUserByEmployeeId(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllEmployeesIsActive(bool? status)
        {
            var employees = await _user.GetAllEmployeesIsActive(status);
            return employees;
        }


        [HttpPut]
        public async Task UpdateEmployeeIsActive(IsActiveModel EmployeeEditByActive, bool Is_Active)
        {
            await _user.UpdateEmployeeIsActive(EmployeeEditByActive, Is_Active);
        }
    }
}
