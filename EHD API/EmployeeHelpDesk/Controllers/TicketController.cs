using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHD.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;

        public TicketController(ITicket ticket) 
        {
            _ticket = ticket;
        }


        [HttpPost]
        public async Task CreateTicket(CreateTicketDTO ticketModel)
        {

            await _ticket.CreateTicket(ticketModel);
        }


        [HttpPut]
        public async Task<IActionResult> UpadteFeedback(string ticketId, [FromBody] FeedbackDTO feedback)
        {
            if (string.IsNullOrEmpty(ticketId))
                return BadRequest("TicketId is required.");

            var result = await _ticket.UpadteFeedback(ticketId, feedback);

            if (!result)
                return NotFound($"Feedback with ticket id {ticketId} not found.");

            return Ok("Feedback updated successfully.");
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllActiveTickets(string departmentId)
        {
            return await _ticket.GetAllActiveTickets(departmentId);
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllOverDueTickets(string departmentId)
        {
            return await _ticket.GetAllOverDueTickets(departmentId);
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllClosedTickets(string departmentId)
        {
            return await _ticket.GetAllClosedTickets(departmentId);
        }

        [HttpPut]
        public async Task UpdateTicketStatus(UpdateTicketStatusDTO ticketStatus)
        {
            await _ticket.UpdateTicketStatus(ticketStatus);
        }

        [HttpPut]
        public async Task UpdateTicketDepartment(UpdateDepartmentTicketDTO data)
        {
            await _ticket.UpdateTicketDepartment(data);
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllRejectedTickets(string departmentId)
        {
            return await _ticket.GetAllRejectedTickets(departmentId);
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetAllReRaisedTickets(string departmentId)
        {
            return await _ticket.GetAllReRaisedTickets(departmentId);
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetUnresolvedTicketsByDepartmentId(string departmentId)
        {
            return await _ticket.GetUnresolvedTicketsByDepartmentId(departmentId);
        }

        [HttpGet]
        public async Task<IQueryable<GetTicketByDepartmentDTO>> GetRepeatedlyReRaisedTicketsByDepartmentId(string departmentId)
        {
            return await _ticket.GetRepeatedlyReRaisedTicketsByDepartmentId(departmentId);
        }
    }
}
