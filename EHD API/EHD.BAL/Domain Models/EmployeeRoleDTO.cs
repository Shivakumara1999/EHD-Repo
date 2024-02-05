using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class EmployeeRoleDTO
    {
        public string RoleId { get; set; }
    }
    public class EmployeeRolenameDTO
    {
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string Gender { get; set; }

        public string OfficialMailId { get; set; }

        public string? AlternateMailId { get; set; }

        public string ContactNumber { get; set; }

        public string Location { get; set; }

        public DateTime JoiningDate { get; set; }

        public int Salary { get; set; }

        public bool IsActive { get; set; } = true;

        public string? RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
