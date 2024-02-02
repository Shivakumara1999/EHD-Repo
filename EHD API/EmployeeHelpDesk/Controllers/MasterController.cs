using EHD.BAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHD.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMaster _master;

        public MasterController(IMaster master) 
        {
            _master = master;
        }
    }
}
