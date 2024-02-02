using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Priority
    {
        [Key]
        public int PriorityId { get; set; }

        public string PriorityName { get; set; }

        public bool IsActive { get; set; }
    }
}
