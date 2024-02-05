using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class getTicketsByEmpIdDTO
    {
        public string TicketId { get; set; }
        public string TicketDescription { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string DepartmentName { get; set; }
        public string? IssueName { get; set; }
        public string StatusName { get; set; }
        public string FeedbackType { get; set; }
        public string Assignee { get; set; }
        public string AssigneeId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public int? IssueId { get; set; }
        public int PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? FeedbackId { get; set; }
        public string? FeedbackDescription { get; set; }
        public bool ReRaiseStatus { get; set; }
        public int? ReRaiseCount { get; set; }
        public string? ReRaiseReason { get; set; }
        public string? RejectedReason { get; set; }
        public string? ResolvedReason { get; set; }
        public DateTime? ReRaiseDate { get; set; }
        public DateTime? RejectedDate { get; set; }
    }
    
}
