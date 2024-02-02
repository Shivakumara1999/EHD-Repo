using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Role : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        [ForeignKey("DepartmentId")]
        public string DepartmentId { get; set; }

    }
}
