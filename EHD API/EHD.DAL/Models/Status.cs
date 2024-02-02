using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public bool IsActive { get; set; }

    }
}
