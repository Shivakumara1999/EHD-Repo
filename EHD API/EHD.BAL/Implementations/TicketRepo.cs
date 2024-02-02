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

        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllActiveTickets(string departmentId)
        {
            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && (t.StatusId == null || t.StatusId == 1))
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.DueDate
                }).ToListAsync();

            return data.AsQueryable();
        }

        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllOverDueTickets(string departmentId)
        {
            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && (t.StatusId == null || t.StatusId == 1)
                && DateTime.Now > t.DueDate
                && DateTime.Now < t.DueDate.AddDays(1))
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.DueDate.AddDays(1)
                }).ToListAsync();

            return data.AsQueryable();
        }

        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllClosedTickets(string departmentId)
        {
            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && t.StatusId == 3)
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.ResolvedDate,
                    StatusMessage = t.ResolvedReason
                }).ToListAsync();

            return data.AsQueryable();
        }

        public async Task UpdateTicketStatus(UpdateTicketStatusDTO ticketStatus)
        {
            var ticket = _dbContext.tickets.FirstOrDefault(t => t.TicketId == ticketStatus.TicketId);

            if (ticket != null)
            {
                ticket.StatusId = ticketStatus.StatusId;
                switch (ticketStatus.StatusId)
                {
                    case 1:
                        ticket.Assignee = ticketStatus.Assignee;
                        break;
                    case 2:
                        ticket.RejectedReason = ticketStatus.Reason;
                        break;
                    case 3:
                        ticket.ResolvedReason = ticketStatus.Reason;
                        break;
                }
                _dbContext.tickets.Update(ticket);
                await _dbContext.SaveChangesAsync();

            }
        }

        public async Task UpdateTicketDepartment(UpdateDepartmentTicketDTO data)
        {
            var ticket = await _dbContext.tickets.FirstOrDefaultAsync(t => t.TicketId == data.TicketId);
            if (ticket != null)
            {
                ticket.DepartmentId = data.DepartmentId;
                _dbContext.tickets.Update(ticket);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllRejectedTickets(string departmentId)
        {
            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && t.StatusId == 2)
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.RejectedDate,
                    StatusMessage = t.RejectedReason
                }).ToListAsync();

            return data.AsQueryable();
        }

        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllReRaisedTickets(string departmentId)
        {
            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && t.ReRaiseStatus == true)
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.RejectedDate,
                    StatusMessage = t.ReRaiseReason,
                    ReRaiseCount = t.ReRaiseCount
                }).ToListAsync();

            return data.AsQueryable();
        }

        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetUnresolvedTicketsByDepartmentId(string departmentId)
        {


            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && (t.StatusId == 1 || t.StatusId == null) && t.DueDate.AddDays(1) < DateTime.Now)
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    EmployeeId = t.EmployeeId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.RejectedDate,
                    StatusMessage = t.RejectedReason,
                    Assignee = t.Assignee
                }).ToListAsync();

            return data.AsQueryable();
        }

        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetRepeatedlyReRaisedTicketsByDepartmentId(string departmentId)
        {
            var data = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(p => p.Priority)
                .Include(s => s.Status)
                .Where(t => t.DepartmentId == departmentId && t.ReRaiseCount > 1 && t.ReRaiseStatus == true)
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new GetTicketByDepartmentDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.RejectedDate,
                    StatusMessage = t.ReRaiseReason,
                    ReRaiseCount = t.ReRaiseCount
                }).ToListAsync();

            return data.AsQueryable();
        }

}
}
