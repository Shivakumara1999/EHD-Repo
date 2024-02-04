using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EHD.BAL.Domain_Models.DepartmentDTO;
using static EHD.BAL.Domain_Models.RoleDTO;

namespace EHD.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMaster _master;

        public MasterController(IMaster master)
        {
            _master = master;
        }

        //Department

        [HttpPost]
        public async Task AddOrUpdateDepartmentAsync(AddDepartment department)
        {
            await _master.AddOrUpdateDepartmentAsync(department);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _master.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpPut]
        public async Task EditDepartmentIsActiveAsync(IsActiveModel DepartmentEditByActive, bool Is_Active)
        {
            await _master.EditDepartmentsIsActiveAsync(DepartmentEditByActive, Is_Active);
        }

        [HttpGet]
        public async Task<IEnumerable<Department>> GetDepartmentsByActiveAsync(bool isActive)
        {
            var departments = await _master.GetDepartmentsByActiveAsync(isActive);
            return departments;
        }


        //Roles

        [HttpPost]
        public async Task AddorUpdateRolesAsync(AddRole role)
        {
            await _master.AddorUpdateRolesAsync(role);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRoleIsActive(IsActiveModel RolesEditByActive, bool Is_Active)
        {
            await _master.UpdateRoleIsActive(RolesEditByActive, Is_Active);
            return Ok();
        }


        [HttpGet]
        public async Task<IEnumerable<GetRoleDTO>> GetAllRoles(bool isActive)
        {
            var roles = await _master.GetAllRoles(isActive);
            return roles;
        }

        //Counts

        [HttpGet]
        public async Task<IActionResult> GetUnresolvedTicketCountsAsync()
        {
            var unresolvedCounts = await _master.GetUnresolvedTicketCountsAsync();
            return Ok(unresolvedCounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetReRaisedTicketCountsAsync()
        {
            var reRaisedCounts = await _master.GetReRaisedTicketCountsAsync();
            return Ok(reRaisedCounts);
        }

        [HttpGet]
        public async Task<ActionResult<List<Feedback>>> GetActiveFeedback()
        {
            var activeFeedback = await _master.GetActiveFeedback();
            if (activeFeedback == null)
            {
                return NotFound();
            }
            return activeFeedback;
        }

        [HttpGet]
        public async Task<ActionResult<List<Priority>>> GetActivePriority()
        {
            var activeFeedback = await _master.GetActivePriority();
            if (activeFeedback == null)
            {
                return NotFound();
            }
            return activeFeedback;
        }


        [HttpGet]
        public async Task<ActionResult<List<Status>>> GetActiveStatus()
        {
            var activeFeedback = await _master.GetActiveStatus();
            if (activeFeedback == null)
            {
                return NotFound();
            }
            return activeFeedback;
        }
        //Issues


        [HttpGet]
        public async Task<IEnumerable<GetAllIssueTypesDTO>> GetAllIssueTypes(bool isActive)
        {
            var issues = await _master.GetAllIssueTypes(isActive);
            return issues;
        }

        [HttpGet]
        public async Task<IEnumerable<Issue>> GetActiveIssueType()
        {
            var issues = await _master.GetActiveIssueType();
            return issues.ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRole(Role role)
        {
            await Task.Run(() => _master.AddOrUpdateRole(role));

            return Ok("Role added or updated successfully.");
        }

        [HttpGet]
        public IEnumerable<Department> GetActiveDepartments()
        {
            var activeDepts = _master.GetActiveDepartments();
            return activeDepts;
        }

        [HttpPost]
        public async Task<IActionResult> AddIssueTypes(List<IssuesDTO> issues)
        {
            await _master.AddIssueTypes(issues);
            return Ok("Issue types added successfully.");
        }

        [HttpPut("UpdateIssueType")]
        public async Task<IActionResult> UpdateIssueTypes(IssuesDTO issue)
        {
            await _master.UpdateIssueTypes(issue);
            return Ok("Issue type updated successfully.");
        }

        [HttpGet]
        public async Task<IQueryable> GetAllDepartmentName()
        { 
           return await _master.GetAllDepartmentName();
        }

        [HttpPut]
        public void EditIssueIsActive(IsActiveModel IssueEditByActive, bool Is_Active)
        {
            _master.EditIssueIsActive(IssueEditByActive, Is_Active);
        }



        [HttpPut]
        public void EditRolesIsActive(IsActiveModel RolesEditByActive, bool Is_Active)
        {
            _master.EditRolesIsActive(RolesEditByActive, Is_Active);
        }

        [HttpGet]
        public async Task<IEnumerable<DepartmentIdNameDto>> GetAllDepartmentsByRoles(string roleId)
        {
           return await  _master.GetDepartmentByRoleId( roleId);
        }
    }
}
