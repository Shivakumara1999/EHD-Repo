using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using EHD.DAL.Models;
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

        public UserRepo(EHDContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddOrUpdateEmployee(Employee employee)
        {
            var existingEmployeeId = await _dbContext.employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId && e.EmployeeId != employee.EmployeeId);

            if (existingEmployeeId != null)
            {
                throw new EmployeeIdExistException();
            }

            char[] allowedGenders = { 'F', 'M' };

            if (!allowedGenders.Contains(employee.Gender))
            {
                throw new InvalidGenderException();
            }
            if (employee.OfficialMailId == employee.AlternateMailId)
            {
                throw new EmployeeEmailAlternateException();
            }

            var existingContactNumber = await _dbContext.employees.FirstOrDefaultAsync(e => e.ContactNumber == employee.ContactNumber && e.EmployeeId != employee.EmployeeId);

            if (existingContactNumber != null)
            {
                throw new EmployeeContactExistException();
            }

            var existingEmployee = await _dbContext.employees.FirstOrDefaultAsync(e => e.OfficialMailId == employee.OfficialMailId);

            if (existingEmployee == null)
            {
                employee.Password = BCrypt.Net.BCrypt.HashPassword("Joyit@321");
                _dbContext.employees.Add(employee);
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
                existingEmployee.Designation = employee.Designation;
                existingEmployee.RoleId = employee.RoleId;
                existingEmployee.DepartmentId = employee.DepartmentId;
                existingEmployee.Gender = employee.Gender;

                _dbContext.Entry(existingEmployee).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
            return employee.OfficialMailId;
        }

        public async Task<Employee> GetUserByEmployeeId(string employeeId)
        {
            return await _dbContext.employees.FindAsync(employeeId);
        }

        public async Task<IQueryable<Employee>> GetAllEmployeesIsActive(bool? status)
        {
            IQueryable<Employee> query = _dbContext.employees;

            if (status.HasValue)
            {
                query = status.Value
                    ? query.Where(e => e.IsActive)
                    : query.Where(e => !e.IsActive);
            }

            List<Employee> employees = await query.ToListAsync();
            return employees.AsQueryable();
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
    }
}
