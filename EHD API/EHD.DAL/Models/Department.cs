using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Department : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

    }
}
