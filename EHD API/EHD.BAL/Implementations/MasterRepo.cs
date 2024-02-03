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
using static EHD.BAL.Domain_Models.RoleDTO;
using static EHD.BAL.Exceptions.AllExceptions;

namespace EHD.BAL.Implementations
{
    public class MasterRepo : IMaster
    {
        private readonly EHDContext _dbContext;

        public MasterRepo(EHDContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Department

        public async Task AddOrUpdateDepartmentAsync(AddDepartment department)
        {
            var DepartmentById = await _dbContext.departments.FirstOrDefaultAsync(e => e.DepartmentId == department.DepartmentId);
            var DepartmentByName = await _dbContext.departments.FirstOrDefaultAsync(e => e.DepartmentName == department.DepartmentName);

            if (DepartmentById == null)
            {
                if (DepartmentByName == null)
                {
                    var newDepartment = new Department
                    {
                        DepartmentId = department.DepartmentId,
                        DepartmentName = department.DepartmentName,
                        CreatedBy = department.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.departments.Add(newDepartment);
                }
                else
                {
                    throw new DepartmentIdNotExistException();
                }
            }
            else
            {
                DepartmentById.DepartmentId = department.DepartmentId;

                if (DepartmentByName == null)
                {
                    DepartmentById.DepartmentName = department.DepartmentName;
                    DepartmentById.ModifiedBy = department.ModifiedBy;
                    DepartmentById.ModifiedDate = DateTime.Now;
                }
                else
                {
                    throw new DepartmentNameExistException();
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _dbContext.departments.ToListAsync();
        }

        public async Task EditDepartmentsIsActiveAsync(IsActiveModel DepartmentEditByActive, bool Is_Active)
        {
            var departmentIds = DepartmentEditByActive.Id;

            var departmentsToUpdate = await _dbContext.departments
                .Where(d => departmentIds.Contains(d.DepartmentId))
                .ToListAsync();

            foreach (var department in departmentsToUpdate)
            {
                department.IsActive = Is_Active;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByActiveAsync(bool isActive)
        {
            return await _dbContext.departments.Where(d => d.IsActive == isActive).ToListAsync();
        }


        //Roles

        public async Task AddorUpdateRolesAsync(AddRole role)
        {
            var RolesById = await _dbContext.roles.FirstOrDefaultAsync(e => e.RoleId == role.RoleId);
            var RolesByName = await _dbContext.roles.FirstOrDefaultAsync(e => e.RoleName == role.RoleName);

            if (RolesById == null)
            {
                if (RolesByName == null)
                {
                    var newRole = new Role
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName,
                        CreatedBy = role.CreatedBy,
                        CreatedDate = DateTime.Now,
                        DepartmentId = role.DepartmentId
                    };
                    _dbContext.roles.Add(newRole);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new RoleIdNotExistException();
                }
            }
            else
            {
                RolesById.RoleId = role.RoleId;

                if (RolesByName != null)
                {
                    RolesById.RoleName = role.RoleName;
                    RolesById.ModifiedBy = role.ModifiedBy;
                    RolesById.ModifiedDate = DateTime.Now;
                }
                else
                {
                    throw new RoleNameNotExistException();
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateRoleIsActive(IsActiveModel RolesEditByActive, bool Is_Active)
        {
            var RolesIds = RolesEditByActive.Id;

            var RolesToUpdate = _dbContext.roles
                .Where(d => RolesIds.Contains(d.RoleId))
                .ToList();

            foreach (var roles in RolesToUpdate)
            {
                roles.IsActive = Is_Active;
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<Role>> GetAllRoles(bool isActive)
        {
            return await _dbContext.roles.Where(d => d.IsActive == isActive).ToListAsync();
        }

        //Counts

        public async Task<string> GetUnresolvedTicketCountsAsync()
        {
            var unresolvedCounts = await _dbContext.departments
                .GroupJoin(
                    _dbContext.tickets
                        .Where(t => t.IsActive == true && (t.StatusId == null || t.StatusId == 1) && DateTime.Now > t.DueDate.AddDays(1)),
                    department => department.DepartmentId,
                    ticket => ticket.DepartmentId,
                    (department, tickets) => new
                    {
                        DepartmentId = department.DepartmentId,
                        DepartmentName = department.DepartmentName,
                        UnresolvedCount = tickets.Count()
                    }
                )
                .ToListAsync();

            return System.Text.Json.JsonSerializer.Serialize(unresolvedCounts);
        }

        public async Task<string> GetReRaisedTicketCountsAsync()
        {
            var reRaisedCounts = await _dbContext.departments
                .GroupJoin(
                    _dbContext.tickets
                        .Where(t => t.IsActive == true && t.ReRaiseStatus && t.ReRaiseCount > 1),
                    department => department.DepartmentId,
                    ticket => ticket.DepartmentId,
                    (department, tickets) => new
                    {
                        DepartmentId = department.DepartmentId,
                        DepartmentName = department.DepartmentName,
                        ReRaisedCount = tickets.Count()
                    }
                )
                .ToListAsync();

            return System.Text.Json.JsonSerializer.Serialize(reRaisedCounts);
        }

        public async Task<List<Feedback>> GetActiveFeedback()
        {
            return await _dbContext.feedbacks.Where(f => f.IsActive == true).ToListAsync();
        }

        public async Task<List<Priority>> GetActivePriority()
        {
            return await _dbContext.priorities.Where(f => f.IsActive == true).ToListAsync();
        }

        public async Task<List<Status>> GetActiveStatus()
        {
            return await _dbContext.status.Where(f => f.IsActive == true).ToListAsync();
        }

        //issues
        public async Task<IEnumerable<Issue>> GetAllIssueTypes(bool isActive)
        {
            return await _dbContext.issues.Where(d => d.IsActive == isActive).ToListAsync();
        }


        public async Task<IEnumerable<Issue>> GetActiveIssueType()
        {
            return await _dbContext.issues.Where(d => d.IsActive == true).ToListAsync();
        }


        public async Task AddOrUpdateRole(Role role)
        {
            var RolesById = await _dbContext.roles.FirstOrDefaultAsync(e => e.RoleId == role.RoleId);
            var RolesByName = await _dbContext.roles.FirstOrDefaultAsync(e => e.RoleName == role.RoleName);

            if (RolesById == null)
            {
                {
                    if (RolesByName == null)
                    {
                        var newRole = new Role
                        {
                            RoleId = role.RoleId,
                            RoleName = role.RoleName,
                            CreatedBy = role.CreatedBy,
                            CreatedDate = DateTime.Now,
                            DepartmentId = role.DepartmentId
                        };
                        _dbContext.roles.Add(newRole);
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        throw new RoleIdNotExistException();
                    }
                }
            }
            else
            {
                RolesById.RoleId = role.RoleId;

                if (RolesByName != null)
                {
                    RolesById.RoleName = role.RoleName;
                    RolesById.ModifiedBy = role.ModifiedBy;
                    RolesById.ModifiedDate = DateTime.Now;

                }
                else
                {
                    throw new RoleNameNotExistException();
                }
                await _dbContext.SaveChangesAsync();
            }
        }


        public IEnumerable<Department> GetActiveDepartments()
        {
            return _dbContext.departments
                .Where(d => d.IsActive)
                .Select(d => new Department { DepartmentId = d.DepartmentId, DepartmentName = d.DepartmentName, IsActive =d.IsActive })
                .ToList();
        }

        public async Task AddIssueTypes(List<IssuesDTO> issues)
        {
            foreach (var issueDto in issues)
            {
                var issue = new Issue
                {
                    IssueName = issueDto.IssueName,
                    DepartmentId = issues[0].DepartmentId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    CreatedBy = issueDto.EmployeeId
                };

                _dbContext.issues.Add(issue);

                await _dbContext.SaveChangesAsync();
            }
        }

        public void EditIssueIsActive(IsActiveModel IssueEditByActive, bool Is_Active)
        {
            var IssueIds = IssueEditByActive.Id.Select(int.Parse).ToList();

            var IssueToUpdate = _dbContext.issues
                .Where(d => IssueIds.Contains(d.IssueId))
                .ToList();

            foreach (var issues in IssueToUpdate)
            {
                issues.IsActive = Is_Active;
            }

            _dbContext.SaveChanges();
        }

        public void EditRolesIsActive(IsActiveModel RolesEditByActive, bool Is_Active)
        {
            var RolesIds = RolesEditByActive.Id;

            var RolesToUpdate = _dbContext.roles
                .Where(d => RolesIds.Contains(d.RoleId))
                .ToList();

            foreach (var roles in RolesToUpdate)
            {
                roles.IsActive = Is_Active;
            }

            _dbContext.SaveChanges();
        }

    

}
}
