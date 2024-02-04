using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.AccountsDTO;

namespace EHD.BAL.Interface
{
    public interface IAccount
    {
        string Login(LoginDTO model);
         Task OtpGeneration(string email);
        void ForgotPassword(ForgotPasswordDTO forgot);
        void ChangePassword(ChangePasswordDTO change);
    }
}
