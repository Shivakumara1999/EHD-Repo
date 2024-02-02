using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class MailTemplateDTO
    {
        public string? ToAddress { get; set; }
        public string? Subject { get; set; }
        public string? MailHeader { get; set; }
        public string? MailBody { get; set; }
        public string? MailFooter { get; set; }
    }
}
