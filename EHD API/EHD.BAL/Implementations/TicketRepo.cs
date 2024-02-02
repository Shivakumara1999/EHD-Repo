using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Implementations
{
    public class TicketRepo : ITicket
    {
        private readonly EHDContext _dbContext;

        public TicketRepo(EHDContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> UpadteFeedback(string ticketId, FeedbackDTO feedback)
        {
            var existingFeedback = await _dbContext.tickets.FindAsync(ticketId);

            if (existingFeedback == null)
                return false;

            existingFeedback.FeedbackDescription = feedback.FeedbackDescription;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
