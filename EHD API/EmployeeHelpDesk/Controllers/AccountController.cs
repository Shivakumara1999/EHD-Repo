using EHD.BAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EHD.BAL.Domain_Models.AccountsDTO;
using static EHD.BAL.Exceptions.AllExceptions;

namespace EHD.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomExceptionFilter]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _accountsContext;

        public AccountController(IAccount account) 
        {
            _accountsContext = account;       
        }
        [HttpPost]
        public IActionResult LoginCandidate(LoginDTO login)
        {
            return Ok(_accountsContext.Login(login));
        }
        [HttpPost]
        public void OtpGeneration(string email)
        {
            _accountsContext.OtpGeneration(email);
        }
        [HttpPost]
        [Route("VerifyOTP")]
        public void VerifyOTP(ForgotPasswordDTO forgot)
        {
            _accountsContext.ForgotPassword(forgot);
        }

        [HttpPost]
        public void ResetPassword(ChangePasswordDTO change)
        {
            _accountsContext.ChangePassword(change);
        }
    }
}
