using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class CreateTicketDTO
    {
        public string? TicketDescription { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public int IssueId { get; set; }
        public int PriorityId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool ReRaiseStatus { get; set; }

    }
    public class GetTicketDTO
    {
        public string TicketId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ContactNumber { get; set; }
        public string Location { get; set; }
        public string TicketDescription { get; set; }
        public string Department { get; set; }
        public string Issue { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public bool ReRaiseStatus { get; set; }
        public int? ReRaiseCount { get; set; }
        public string StatusMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? TicketDate { get; set; }
        public string EmployeeId { get; set; }
        public string? Assignee { get; set; }

    }

    public class GetTicketByDepartmentDTO
    {
        public string TicketId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ContactNumber { get; set; }
        public string Location { get; set; }
        public string TicketDescription { get; set; }
        public string Department { get; set; }
        public string Issue { get; set; }
        public int? PriorityId { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public bool ReRaiseStatus { get; set; }
        public int? ReRaiseCount { get; set; }
        public string StatusMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? TicketDate { get; set; }
        public string EmployeeId { get; set; }
        public string? AssigneeId { get; set; }
        public string? Assignee { get; set; }

    }

    public class UpdateDepartmentTicketDTO
    {
        public string TicketId { get; set; }
        public string DepartmentId { get; set; }
    }

    public class UpdateTicketStatusDTO
    {
        public string TicketId { get; set; }
        public int? StatusId { get; set; }
        public string? AssigneeId { get; set; }
        public string? Reason { get; set; }
        //public string EmployeeId { get; set; }
    }
    public class AdminReRaiseTicketDTO
    {
        public string TicketId { get; set; }
        public string ModifiedBy { get; set; }
        public string? ReRaiseReason { get; set; }
    }

}
