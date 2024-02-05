using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.DepartmentDTO;
using static EHD.BAL.Exceptions.AllExceptions;

namespace EHD.BAL.Implementations
{
    public class UserRepo : IUser
    {
        private readonly EHDContext _dbContext;
        private readonly IMailTemplate _mail;
        public UserRepo(EHDContext dbContext, IMailTemplate mail)
        {
            _dbContext = dbContext;
            _mail = mail;
        }

        public async Task<string> AddOrUpdateEmployee(Employee employee)
        {
            var existingEmployeeId = await _dbContext.employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId && e.EmployeeId != employee.EmployeeId);

            if (existingEmployeeId != null)
            {
                throw new EmployeeIdExistException();
            }

            string[] allowedGenders = { "F", "M" };

            if (!allowedGenders.Contains(employee.Gender))
            {
                throw new InvalidGenderException();
            }
            if (employee.OfficialMailId == employee.AlternateMailId)
            {
                throw new EmployeeEmailAlternateException();
            }

            
            var existEmployee = await _dbContext.employees.FirstOrDefaultAsync(e => e.OfficialMailId == employee.OfficialMailId && e.EmployeeId != employee.EmployeeId);

            if (existEmployee != null)

            {

                throw new EmployeeEmailExistException();

            }
            var existingContactNumber = await _dbContext.employees.FirstOrDefaultAsync(e => e.ContactNumber == employee.ContactNumber && e.EmployeeId != employee.EmployeeId);

            if (existingContactNumber != null)
            {
                throw new EmployeeContactExistException();
            }

            var existingEmployee = await _dbContext.employees.FirstOrDefaultAsync(e => e.OfficialMailId == employee.OfficialMailId);

            if (existingEmployee == null)
            {
                employee.CreatedDate = DateTime.Now;
                string autopassw = "Joyit@321";
                employee.Password = BCrypt.Net.BCrypt.HashPassword(autopassw);
                _dbContext.employees.Add(employee);
                var newUser = new MailTemplateDTO
                {
                    ToAddress = employee.OfficialMailId,
                    Subject = "HelpDesk Login Credentials",
                    MailHeader = autopassw,
                    MailBody = $"Dear, {employee.FirstName + " " + employee.LastName}, <br><br/> Here is the password to access your HelpDesk. Please keep it confidential, and remember that you have the option to change it at any time. <br></br> Please find the below link to raise ticket from Joy Help desk. <br></br> <a>http://localhost:3000/</a>",
                    MailFooter = "From Joy Help Desk team! "
                };
                _mail.SendMail(newUser);
            }
            else
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.AlternateMailId = employee.AlternateMailId;
                existingEmployee.ContactNumber = employee.ContactNumber;
                existingEmployee.Location = employee.Location;
                existingEmployee.JoiningDate = employee.JoiningDate;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.DesignationId = employee.DesignationId;
                existingEmployee.RoleId = employee.RoleId;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.ModifiedDate = DateTime.Now;
                existingEmployee.ModifiedBy = employee.ModifiedBy;

                _dbContext.Entry(existingEmployee).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
            return employee.OfficialMailId;
        }

        public async Task<Employee> GetUserByEmployeeId(string employeeId)
        {
            return await _dbContext.employees.FindAsync(employeeId);
        }

        public async Task<IEnumerable<EmployeeRolenameDTO>> GetAllEmployeesIsActive(bool? status)
        {
            var employeeWithRoles = await _dbContext.employees
                .Where(e => e.IsActive == status)
                .Join(_dbContext.roles,
                    emp => emp.RoleId,
                    role => role.RoleId,
                    (emp, role) => new EmployeeRolenameDTO
                    {
                        EmployeeId = emp.EmployeeId,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Gender = emp.Gender,
                        OfficialMailId = emp.OfficialMailId,
                        AlternateMailId = emp.AlternateMailId,
                        ContactNumber = emp.ContactNumber,
                        Location = emp.Location,
                        Salary = emp.Salary,
                        IsActive = emp.IsActive,
                        JoiningDate = emp.JoiningDate,
                        RoleId = emp.RoleId,
                        RoleName = role.RoleName,
                    })
                .ToListAsync();

            return employeeWithRoles;
        }

        public async Task UpdateEmployeeIsActive(IsActiveModel EmployeeEditByActive, bool Is_Active)
        {
            var EmployeeIds = EmployeeEditByActive.Id;

            var EmployeeToUpdate = await _dbContext.employees
                .Where(e => EmployeeIds.Contains(e.EmployeeId))
            .ToListAsync();

            foreach (var Employee in EmployeeToUpdate)
            {
                Employee.IsActive = Is_Active;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeeRole(string employeeId, EmployeeRoleDTO emprole)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                throw new EmployeeIdNotNullException();
            }

            var employee = await _dbContext.employees.FindAsync(employeeId);

            if (employee != null)
            {
                employee.RoleId = emprole.RoleId;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new EmployeeIdNotExistException();
            }


        }

        public async Task<string> GetAssigneeDetails(string ticketId)
        {
            var Assignee = await (from ticket in _dbContext.tickets
                                  join department in _dbContext.departments on ticket.DepartmentId equals department.DepartmentId
                                  join role in _dbContext.roles on department.DepartmentId equals role.DepartmentId
                                  join employee in _dbContext.employees on role.RoleId equals employee.RoleId
                                  where ticket.TicketId == ticketId
                                  select $"{employee.FirstName} {employee.LastName}")
                                  .FirstOrDefaultAsync();

            return Assignee;
        }

        public async Task<object> GetUserProfile(string mail_id)
        {
            var userData = await (from e in this._dbContext.employees
                                  where e.OfficialMailId == mail_id
                                  select new
                                  {
                                      EmployeeName = e.Gender == "M" ? "Mr." + e.FirstName + " " + e.LastName : "Ms." + e.FirstName + " " + e.LastName,
                                  }).FirstOrDefaultAsync();

            return new { EmployeeName = userData.EmployeeName };
        }
       

    }
}
