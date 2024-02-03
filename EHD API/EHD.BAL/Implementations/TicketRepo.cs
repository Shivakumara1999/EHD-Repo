using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using EHD.DAL.Models;
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

        public async Task CreateTicket(CreateTicketDTO ticketModel)
        {

            Ticket newTicket = GenerateTicket(ticketModel);
            _dbContext.tickets.Add(newTicket);
            await _dbContext.SaveChangesAsync();
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


        private Ticket GenerateTicket(CreateTicketDTO ticketModel)
        {
            var ticketsList = _dbContext.tickets.ToList();
            var recentTicketId = ticketsList.OrderByDescending(t => t.TicketId.Length >= 4 ? int.Parse(t.TicketId.Substring(t.TicketId.Length - 4)) : 0).FirstOrDefault();
            int Digits = recentTicketId != null ? int.Parse(recentTicketId.TicketId.Substring(recentTicketId.TicketId.Length - 4)) : 0;
            var departmentList = _dbContext.departments.FirstOrDefault(d => d.DepartmentId == ticketModel.DepartmentId);
            string department = departmentList != null ? departmentList.DepartmentName.Substring(0, Math.Min(3, departmentList.DepartmentName.Length)) : "TK";

            Ticket newTicket = new Ticket
            {
                TicketId = recentTicketId == null ? $"JOY{department.ToUpper()}0001" : $"JOY{department.ToUpper()}{Digits + 1:D4}",
                TicketDescription = ticketModel.TicketDescription,
                EmployeeId = ticketModel.EmployeeId,
                DepartmentId = ticketModel.DepartmentId,
                IssueId = ticketModel.IssueId,
                PriorityId = ticketModel.PriorityId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = ticketModel.CreatedBy,
                IsActive = ticketModel.IsActive,
                ReRaiseStatus = false,
            };

            switch (ticketModel.PriorityId)
            {
                case 1:
                    newTicket.DueDate = newTicket.CreatedDate.AddDays(7);
                    break;
                case 2:
                    newTicket.DueDate = newTicket.CreatedDate.AddDays(3);
                    break;
                case 3:
                    newTicket.DueDate = newTicket.CreatedDate.AddDays(1);
                    break;
                default:
                    newTicket.DueDate = ticketModel.DueDate;
                    break;
            }

            return newTicket;
        }
    }
}
