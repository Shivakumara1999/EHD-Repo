using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using EHD.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Implementations
{
    public class TicketRepo : ITicket
    {
        private readonly EHDContext _dbContext;
        private readonly IMailTemplate _mail;

        public TicketRepo(EHDContext dbContext, IMailTemplate mail)
        {
            _dbContext = dbContext;
            _mail = mail;
        }

        public async Task CreateTicket(CreateTicketDTO ticketModel)
        {
            var employee = await _dbContext.employees.FirstOrDefaultAsync(e => e.EmployeeId == ticketModel.EmployeeId);

            Ticket newTicket = GenerateTicket(ticketModel);
            _dbContext.tickets.Add(newTicket);
            await _dbContext.SaveChangesAsync();

            var mailData = new MailTemplateDTO
            {
                ToAddress = employee.OfficialMailId,
                Subject = "Ticket has been raised",
                MailHeader = $"Ticket has been raised to respective department",
                MailBody = $"Dear, {employee.FirstName + " " + employee.LastName},<br></br> We would like to inform you that your ticket has been raised successfully to the respective department with ticketid #{newTicket.TicketId}.",
                MailFooter = "From Joy Help Desk team! "
            };

            await _mail.SendMail(mailData);

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
                    EmployeeId= t.EmployeeId,
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
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
                    EmployeeId = t.EmployeeId,
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
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
                    EmployeeId = t.EmployeeId,
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
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
            var assignee = _dbContext.employees.FirstOrDefault(e => e.EmployeeId == ticketStatus.AssigneeId);

            if (ticket != null)
            {
                ticket.StatusId = ticketStatus.StatusId;
                ticket.ModifiedBy = ticketStatus.AssigneeId;
                ticket.ModifiedDate = DateTime.Now;
                switch (ticketStatus.StatusId)
                {
                    case 1:
                        ticket.AssigneeId = assignee.EmployeeId;
                        ticket.Assignee = assignee.FirstName + " " + assignee.LastName;
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
                var employee = await _dbContext.employees.FirstOrDefaultAsync(e => e.EmployeeId == ticket.EmployeeId);
                if (employee != null) {

                    var mailData = new MailTemplateDTO
                    {
                        ToAddress = employee.OfficialMailId,
                        Subject = "Regarding ticket raising to respective department",
                        MailHeader = "Ticket has been raised to respective department",
                        MailBody = $"Dear, {employee.FirstName + " " + employee.LastName},<br></br> We would like to inform you that we have moved your ticket with ticket id #{ticket.TicketId} to the relevent department, since you had raised to irrelevent department.",
                        MailFooter = "From Joy Help Desk team! "
                    };


                    ticket.DepartmentId = data.DepartmentId;
                    ticket.ModifiedBy = data.EmplyoeeId;
                    ticket.ModifiedDate = DateTime.Now;
                    _dbContext.tickets.Update(ticket);
                    await _dbContext.SaveChangesAsync();

                    _mail.SendMail(mailData);
                }

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
                    EmployeeId = t.EmployeeId,
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
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
                    EmployeeId = t.EmployeeId,
                    TicketDescription = t.TicketDescription,
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
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
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
                    TicketDescription = t.TicketDescription,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    PriorityId = t.PriorityId,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.RejectedDate,
                    StatusMessage = t.RejectedReason,
                    AssigneeId = t.AssigneeId,
                    Assignee = t.Assignee,
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
                    UserName = t.Employee.FirstName + " " + t.Employee.LastName,
                    EmployeeId = t.EmployeeId,
                    Department = t.Department.DepartmentName,
                    Issue = t.Issue.IssueName,
                    PriorityId = t.PriorityId,
                    Priority = t.Priority.PriorityName,
                    Status = t.Status.StatusName,
                    ReRaiseStatus = t.ReRaiseStatus,
                    CreatedDate = t.CreatedDate,
                    TicketDate = t.RejectedDate,
                    StatusMessage = t.ReRaiseReason,
                    ReRaiseCount = t.ReRaiseCount,
                    AssigneeId = t.AssigneeId,
                    Assignee = t.Assignee,
                }).ToListAsync();

            return data.AsQueryable();
        }

        public string GetCount(string departmentId)
        {
            var tickets = _dbContext.tickets
                .Where(t => t.IsActive == true && t.DepartmentId == departmentId)
                .ToList();

           // var totalTicketsCount = tickets.Count();
          
            var activeTicketsCount = tickets.Count(t => t.StatusId == null || t.StatusId == 1);
            var overDueTicketsCount = tickets.Count(t => (t.StatusId == null || t.StatusId == 1)
                && DateTime.Now > t.DueDate
                && DateTime.Now < t.DueDate.AddDays(1));
            var closedTicketsCount = tickets.Count(t => t.StatusId == 3);
            var rejectedTicketsCount = tickets.Count(t => t.StatusId == 2);
            var reRaisedTicketsCount = tickets.Count(t => t.ReRaiseStatus == true);
            var totalTicketsCount = activeTicketsCount + reRaisedTicketsCount + rejectedTicketsCount + closedTicketsCount + overDueTicketsCount;

            var data = new
            {
                totalTicketsCount = totalTicketsCount,
                activeTicketsCount = activeTicketsCount,
                overDueTicketsCount = overDueTicketsCount,
                closedTicketsCount = closedTicketsCount,
                rejectedTicketsCount = rejectedTicketsCount,
                reRaisedTicketsCount = reRaisedTicketsCount
            };

            return System.Text.Json.JsonSerializer.Serialize(data);
        }


        public async Task<IQueryable> GetIssueTypeByDepartmentId(string departmentId)
        {
            var query = from issue in _dbContext.issues
                        where issue.DepartmentId == departmentId
                        group issue by new { issue.DepartmentId, issue.IssueName, issue.IssueId } into grouped
                        orderby grouped.Key.DepartmentId
                        select new
                        {
                            issueid = grouped.Key.IssueId,
                            DepartmentId = grouped.Key.DepartmentId,
                            IssueName = grouped.Key.IssueName,
                        };

            var result = await query.ToListAsync();
            return result.AsQueryable();
        }
        public async Task<bool> UpdateTicketAsync(string ticketId, Re_raisedDTO reRaisedDto)
        {
            var ticket = await _dbContext.tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return false;
            }
            if (ticket.StatusId == 3 || ticket.StatusId == 2)
            {
                ticket.ReRaiseReason = reRaisedDto.ReRaiseReason;
                ticket.ModifiedBy = ticket.EmployeeId;
                if (ticket.ReRaiseCount == null)
                {
                    ticket.ReRaiseCount = 1;
                }
                else
                {
                    ticket.ReRaiseCount = ticket.ReRaiseCount + 1;
                }
                ticket.ReRaiseDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<List<getTicketsByEmpIdDTO>> GetTicketDetails(string? Empid)
        {
            var ticketDetails = await _dbContext.tickets
                .Include(e => e.Employee)
                .Include(d => d.Department)
                .Include(i => i.Issue)
                .Include(s => s.Status)
                .Include(f => f.Feedback)
                .Where(t => t.EmployeeId == Empid)
                .Select(t => new getTicketsByEmpIdDTO
                {
                    TicketId = t.TicketId,
                    TicketDescription = t.TicketDescription,
                    ResolvedDate = t.ResolvedDate,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    DepartmentName = t.Department.DepartmentName,
                    IssueName = t.Issue.IssueName,
                    StatusName = t.Status.StatusName,
                    FeedbackType = t.Feedback.FeedbackType,
                    Assignee = t.Assignee,
                    AssigneeId = t.AssigneeId,
                    FeedbackId = t.FeedbackId,
                    StatusId = t.StatusId,
                    PriorityId = t.PriorityId,
                    IssueId = t.IssueId,
                    DepartmentId = t.DepartmentId,
                    EmployeeId = t.EmployeeId,
                    RejectedDate = t.RejectedDate,
                    ReRaiseDate = t.ReRaiseDate,
                    ResolvedReason = t.ResolvedReason,
                    RejectedReason = t.RejectedReason,
                    ReRaiseReason = t.ReRaiseReason,
                    ReRaiseCount = t.ReRaiseCount,
                    ReRaiseStatus = t.ReRaiseStatus,
                    FeedbackDescription = t.FeedbackDescription,
                })
                .ToListAsync();

            return ticketDetails;
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
                CreatedDate = DateTime.Now,
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

        public async Task UpdateAdminReRaiseStatus(AdminReRaiseTicketDTO data)
        {
            var ticket = await _dbContext.tickets.FirstOrDefaultAsync(t => t.TicketId == data.TicketId);

            if (ticket != null)
            {
                ticket.ModifiedBy = data.ModifiedBy;
                ticket.ModifiedDate = DateTime.Now;
                ticket.ReRaiseReason = data.ReRaiseReason;
                ticket.StatusId = null;
                ticket.DueDate = ticket.DueDate.AddDays(1);
                ticket.ReRaiseStatus = true;
                ticket.ReRaiseCount = null;
                _dbContext.tickets.Update(ticket);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
