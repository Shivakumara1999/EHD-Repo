using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Ticket : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string TicketId { get; set; }

        public string? TicketDescription { get; set; }
        public string? AssigneeId { get; set; }
        public string? Assignee { get; set; }

        [ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }
        [ForeignKey("DepartmentId")]

        public string DepartmentId { get; set; }

        [ForeignKey("IssueId")]
        public int? IssueId { get; set; }

        [ForeignKey("PriorityId")]
        public int PriorityId { get; set; }

        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }

        [ForeignKey("FeedbackId")]
        public int? FeedbackId { get; set; }

        public DateTime DueDate { get; set; }

        public string? FeedbackDescription { get; set; }

        public bool ReRaiseStatus { get; set; }

        public int? ReRaiseCount { get; set; }

        public string? ReRaiseReason { get; set; }

        public string? RejectedReason { get; set; }

        public string? ResolvedReason { get; set; }

        public DateTime? ReRaiseDate { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public DateTime? RejectedDate { get; set; }

        public virtual Issue Issue { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Department Department { get; set; }

        public virtual Priority Priority { get; set; }

        public virtual Status Status { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
