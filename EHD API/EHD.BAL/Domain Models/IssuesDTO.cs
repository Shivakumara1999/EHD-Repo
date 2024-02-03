using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class IssuesDTO
    {
        public string DepartmentId { get; set; }

        public string IssueName { get; set; }
        public string EmployeeId { get; set; }
    }
    public class GetAllIssueTypesDTO
    {
        public int IssueId { get; set; }
        public string IssueName { get; set; }
        public string DepartmentName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
