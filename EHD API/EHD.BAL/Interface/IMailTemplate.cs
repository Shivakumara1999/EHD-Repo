using EHD.BAL.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Interface
{
    public interface IMailTemplate
    {
        Task SendMail(MailTemplateDTO data);
    }
}
