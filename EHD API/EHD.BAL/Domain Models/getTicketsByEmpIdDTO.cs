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
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}

