using EHD.BAL.Domain_Models;
using EHD.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.DepartmentDTO;

namespace EHD.BAL.Interface
{
    public interface IUser
    {
        Task<string> AddOrUpdateEmployee(Employee employee);

        Task<Employee> GetUserByEmployeeId(string employeeId);

        Task<IQueryable<Employee>> GetAllEmployeesIsActive(bool? activeStatus);

        Task UpdateEmployeeIsActive(IsActiveModel EmployeeEditByActive, bool Is_Active);
        Task UpdateEmployeeRole(string employeeId, EmployeeRoleDTO emprole);
        Task<string> GetAssigneeDetails(string ticketId);
    }
}
