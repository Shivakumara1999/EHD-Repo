using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


    }
}
