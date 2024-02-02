﻿using EHD.BAL.Domain_Models;
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
    }
}
