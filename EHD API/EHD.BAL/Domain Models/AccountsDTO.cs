using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class AccountsDTO
    {
       
            public class LoginDTO
            {
                public string OfficialMailId { get; set; }
                public string Password { get; set; }

            }
            public class ChangePasswordDTO
            {
                public string OfficialMailId { get; set; }
                public string? oldpassword { get; set; }
                public string? newPassword { get; set; }
            }
            public class ForgotPasswordDTO
            {
                public string? OfficialMailId { get; set; }
                public int? Otp { get; set; }
                public string? newPassword { get; set; }
            }
    }
    
}
