using EHD.BAL.Domain_Models;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.DepartmentDTO;
using static EHD.BAL.Domain_Models.DesignationDTO;
using static EHD.BAL.Domain_Models.RoleDTO;

namespace EHD.BAL.Interface
{
    public interface IMaster
    {
        //Department

        Task AddOrUpdateDepartmentAsync(AddDepartment department);

        Task<IEnumerable<Department>> GetAllDepartmentsAsync();

        Task EditDepartmentsIsActiveAsync(IsActiveModel departmentEditByActive, bool is_Active);

        Task<IEnumerable<Department>> GetDepartmentsByActiveAsync(bool isActive);

        //Designations
        Task AddOrUpdateDesignations(AddDesignation designation);

        Task<IQueryable> GetAllDesignations();

        Task EditDesignationsIsActive(IssueIsActiveModel designationEditByActive);

        Task<IEnumerable<Designations>> GetDesignationsByActive(bool isActive);

        //Roles
        Task AddorUpdateRolesAsync(AddRole role);
        Task UpdateRoleIsActive(IsActiveModel RolesEditByActive, bool Is_Active);


        Task<IEnumerable<GetRoleDTO>> GetAllRoles(bool isActive);
        
        Task<IQueryable> GetAllRoleNames();



        //Counts

        Task<string> GetUnresolvedTicketCountsAsync();

        Task<string> GetReRaisedTicketCountsAsync();

        Task<List<Feedback>> GetActiveFeedback();

        Task<List<Priority>> GetActivePriority();

        Task<List<Status>> GetActiveStatus();

        //issues
        Task<IEnumerable<GetAllIssueTypesDTO>> GetAllIssueTypes(bool isActive);

        Task<IEnumerable<Issue>> GetActiveIssueType();

        
        public IEnumerable<Department> GetActiveDepartments();

        Task AddIssueTypes(List<IssuesDTO> issues);

        Task UpdateIssueTypes(IssuesDTO issue);
        Task<IQueryable> GetAllDepartmentName();

        void EditRolesIsActive(IsActiveModel RolesEditByActive);

        void EditIssueIsActive(IssueIsActiveModel IssueEditByActive);

        Task<IEnumerable<DepartmentIdNameDto>> GetDepartmentByRoleId(string roleId);

    }
}
