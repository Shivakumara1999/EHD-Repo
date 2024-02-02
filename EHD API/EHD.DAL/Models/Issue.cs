using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Issue : Entity
    {
        [Key]
        public int IssueId { get; set; }

        public string IssueName { get; set; }

        [ForeignKey("DepartmentId")]
        public string DepartmentId { get; set; }
    }
}
