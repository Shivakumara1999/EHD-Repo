using EHD.BAL.Exceptions;
using EHD.BAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EHD.BAL.Domain_Models.AccountsDTO;

namespace EHD.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomExceptionFilter]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;

        public AccountController(IAccount account) 
        {
            _account = account;       
        }
        [HttpPost("CandidateLogin")]
        public IActionResult LoginCandidate(LoginDTO login)
        {
            return Ok(_accountsContext.Login(login));
        }
        [HttpPost("ForgotPassword")]
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
        [HttpPost("ChangePassword")]
        public void ResetPassword(ChangePasswordDTO change)
        {
            _accountsContext.ChangePassword(change);
        }
    }
}
