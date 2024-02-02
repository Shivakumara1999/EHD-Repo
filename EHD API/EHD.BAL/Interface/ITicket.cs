using EHD.BAL.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Interface
{
    public interface ITicket
    {
        Task CreateTicket(CreateTicketDTO ticketModel);
        Task<bool> UpadteFeedback(string ticketId, FeedbackDTO feedback);
        Task<IQueryable<GetTicketByDepartmentDTO>> GetAllActiveTickets(string departmentId);
        Task<IQueryable<GetTicketByDepartmentDTO>> GetAllOverDueTickets(string departmentId);
        Task<IQueryable<GetTicketByDepartmentDTO>> GetAllClosedTickets(string departmentId);
        Task UpdateTicketStatus(UpdateTicketStatusDTO ticketStatus);
        Task UpdateTicketDepartment(UpdateDepartmentTicketDTO data);
    }
}
