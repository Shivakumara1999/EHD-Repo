using EHD.BAL.Domain_Models;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.DepartmentDTO;
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

        //Roles
        Task AddorUpdateRolesAsync(AddRole role);

        //Counts

        Task<string> GetUnresolvedTicketCountsAsync();

        Task<string> GetReRaisedTicketCountsAsync();

        Task<List<Feedback>> GetActiveFeedback();

        Task<List<Priority>> GetActivePriority();

        Task<List<Status>> GetActiveStatus();
    }
}
