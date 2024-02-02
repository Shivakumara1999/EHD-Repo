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
    }
}
