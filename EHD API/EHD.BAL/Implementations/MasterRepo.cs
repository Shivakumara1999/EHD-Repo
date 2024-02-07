using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.DepartmentDTO;
using static EHD.BAL.Domain_Models.DesignationDTO;
using static EHD.BAL.Domain_Models.RoleDTO;
using static EHD.BAL.Exceptions.AllExceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EHD.BAL.Implementations
{
    public class MasterRepo : IMaster
    {
        private readonly EHDContext _dbContext;
        private object designations;

        public MasterRepo(EHDContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Department

        public async Task AddOrUpdateDepartmentAsync(AddDepartment department)
        {

            var departmentList = await _dbContext.departments.ToListAsync();
            var recentDepartmentId = departmentList.OrderByDescending(t => t.DepartmentId.Length >= 2 ? GetNumericPart(t.DepartmentId) : 0).FirstOrDefault();
            int Digits = recentDepartmentId != null ? GetNumericPart(recentDepartmentId.DepartmentId) : 0;


            var existingDepartment = await _dbContext.departments.FirstOrDefaultAsync(e => e.DepartmentId == department.DepartmentId);

            if (existingDepartment == null)
            {

                var newDepartment = new Department
                {

                    DepartmentId = recentDepartmentId == null ? $"D01" : $"D{Digits + 1:D2}",
                    DepartmentName = department.DepartmentName,
                    CreatedBy = department.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                _dbContext.departments.Add(newDepartment);

            }
            else

            {
                if (existingDepartment != null)
                {
                    existingDepartment.DepartmentName = department.DepartmentName;
                    existingDepartment.ModifiedBy = department.ModifiedBy;
                    existingDepartment.ModifiedDate = DateTime.Now;
                }

            }

            await _dbContext.SaveChangesAsync();


        }

        private int GetNumericPart(string input)
        {
           
            string numericPart = new string(input.Reverse().TakeWhile(char.IsDigit).Reverse().ToArray());
            return int.TryParse(numericPart, out int result) ? result : 0;
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


        public async Task<IEnumerable<DepartmentIdNameDto>> GetDepartmentByRoleId(string roleId)
        {
            var query = from d in _dbContext.departments
                        join r in _dbContext.roles on d.DepartmentId equals r.DepartmentId
                        join e in _dbContext.employees on r.RoleId equals e.RoleId
                        where r.RoleId == roleId
                        select new DepartmentIdNameDto
                        {
                            DepartmentId = d.DepartmentId,
                            DepartmentName = d.DepartmentName
                        };

            return await query.ToListAsync();
        }


        //Desigantions

        public async Task AddOrUpdateDesignations(AddDesignation designation)
        {
            var existingDesignation = await _dbContext.designations.FirstOrDefaultAsync(e => e.DesignationId == designation.DesignationId);

            if (existingDesignation == null)
            {
                
                var newDesignation = new Designations
                {
                    Designation = designation.Designation,
                    CreatedBy = designation.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                _dbContext.designations.Add(newDesignation);
            }
            else
            {
                existingDesignation.Designation = designation.Designation;
                existingDesignation.ModifiedBy = designation.ModifiedBy;
                existingDesignation.ModifiedDate = DateTime.Now;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IQueryable> GetAllDesignations()
        {
           
                var query = from designations in _dbContext.designations
                            select new
                            {
                                DesignationId = designations.DesignationId,
                                Designation = designations.Designation
                            };
                return query;
            
        }

        public async Task EditDesignationsIsActive(IssueIsActiveModel designationEditByActive)
        {
            var DesignationIds = designationEditByActive.Id.ToList();

            var DesignationToUpdate = _dbContext.designations
                .Where(d => DesignationIds.Contains(d.DesignationId))
                .ToList();

            foreach (var designation in DesignationToUpdate)
            {
                designation.IsActive = designationEditByActive.Is_Active;
            }

            _dbContext.SaveChanges();
        }


        public async Task<IEnumerable<Designations>> GetDesignationsByActive(bool isActive)
        {
            return await _dbContext.designations.Where(d => d.IsActive == isActive).ToListAsync();
        }


        //Roles

        public async Task AddorUpdateRolesAsync(AddRole role)
        {
            var roleList = await _dbContext.roles.ToListAsync();


            var recentRoleId = roleList
                .OrderByDescending(t => t.RoleId.Length >= 2 ? GetNumericPart(t.RoleId) : 0)
                .FirstOrDefault();


            int Digits = recentRoleId != null ? GetNumericPart(recentRoleId.RoleId) : 0;


            var existingrole = await _dbContext.roles.FirstOrDefaultAsync(e => e.RoleId == role.RoleId);

            if (existingrole == null)
            {

                var newRole = new Role
                {

                    RoleId = recentRoleId == null ? $"R01" : $"R{Digits + 1:D2}",
                    RoleName = role.RoleName,
                    DepartmentId = role.DepartmentId,
                    CreatedBy = role.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                _dbContext.roles.Add(newRole);
            }
            else
            {

                if (existingrole != null)
                {
                    existingrole.RoleId = role.RoleId;
                    existingrole.RoleName = role.RoleName;
                    existingrole.DepartmentId = role.DepartmentId;
                    existingrole.ModifiedBy = role.ModifiedBy;
                    existingrole.ModifiedDate = DateTime.Now;
                }
            }


            await _dbContext.SaveChangesAsync();
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
        public async Task<IQueryable> GetAllRoleNames()
        {
            var query = from role in _dbContext.roles
                        select new
                        {
                            RoleId = role.RoleId,
                            RoleName = role.RoleName
                        };
            return query;
        }


        public async Task<IEnumerable<GetRoleDTO>> GetAllRoles(bool isActive)
        {
            var rolesWithDepartments = await _dbContext.roles
                .Where(r => r.IsActive == isActive)
                .Join(_dbContext.departments,
                    role => role.DepartmentId,
                    department => department.DepartmentId,
                    (role, department) => new GetRoleDTO
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName,
                        DepartmentName = department.DepartmentName,
                        CreatedBy = role.CreatedBy,
                        CreatedDate = role.CreatedDate
                    })
                .ToListAsync();

            return rolesWithDepartments;
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
        public async Task<IEnumerable<GetAllIssueTypesDTO>> GetAllIssueTypes(bool isActive)
        {
            var issuesWithDepartments = await _dbContext.issues
                .Where(i => i.IsActive == isActive)
                .Join(_dbContext.departments,
                    issue => issue.DepartmentId,
                    department => department.DepartmentId,
                    (issue, department) => new GetAllIssueTypesDTO
                    {
                        IssueId = issue.IssueId,
                        IssueName = issue.IssueName,
                        DepartmentName = department.DepartmentName,
                        CreatedBy = issue.CreatedBy,
                        CreatedDate = issue.CreatedDate
                    })
                .ToListAsync();

            return issuesWithDepartments;
        }


        public async Task<IEnumerable<Issue>> GetActiveIssueType()
        {
            return await _dbContext.issues.Where(d => d.IsActive == true).ToListAsync();
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
                    CreatedBy = issueDto.CreatedBy,
                    ModifiedBy = issueDto.ModifiedBy,
                };

                _dbContext.issues.Add(issue);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateIssueTypes(IssuesDTO issueDto)
        {
            try
            {
                if (issueDto == null)
                {
                    throw new ArgumentNullException(nameof(issueDto), "Issue DTO cannot be null.");
                }

                var existingIssue = await _dbContext.issues.FirstOrDefaultAsync(i => i.IssueId == issueDto.IssueId);
                if (existingIssue != null)
                {
                    existingIssue.IssueName = issueDto.IssueName;
                    existingIssue.DepartmentId = issueDto.DepartmentId;
                    existingIssue.ModifiedDate = DateTime.Now;
                    existingIssue.IsActive = true;
                    existingIssue.ModifiedBy = issueDto.ModifiedBy;
                    existingIssue.CreatedBy = issueDto.CreatedBy;

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException($"Issue with ID {issueDto.IssueId} not found.");
                }
            }
            catch (Exception ex)
            { // Log the exception to the console
                Console.WriteLine($"An error occurred while updating issue types: {ex}");

                throw new dataisnotfound();
            }


        }


        public async Task<IQueryable> GetAllDepartmentName()
        {
            var query = from department in _dbContext.departments
                        select new
                        {
                            departmentId = department.DepartmentId,
                            departmentname = department.DepartmentName
                        };
                        return query;
        }

        public void EditIssueIsActive(IssueIsActiveModel IssueEditByActive)
        {
            var IssueIds = IssueEditByActive.Id.ToList();

            var IssueToUpdate = _dbContext.issues
                .Where(d => IssueIds.Contains(d.IssueId))
                .ToList();

            foreach (var issues in IssueToUpdate)
            {
                issues.IsActive = IssueEditByActive.Is_Active;
            }

            _dbContext.SaveChanges();
        }

        public void EditRolesIsActive(IsActiveModel RolesEditByActive)
        {
            var RolesIds = RolesEditByActive.Id.ToList();

            var RolesToUpdate = _dbContext.roles
                .Where(d => RolesIds.Contains(d.RoleId))
                .ToList();

            foreach (var roles in RolesToUpdate)
            {
                roles.IsActive = RolesEditByActive.Is_Active;
            }

            _dbContext.SaveChanges();
        }



    }
}
