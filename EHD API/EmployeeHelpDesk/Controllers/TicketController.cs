using EHD.BAL.Interface;
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
    }
}
