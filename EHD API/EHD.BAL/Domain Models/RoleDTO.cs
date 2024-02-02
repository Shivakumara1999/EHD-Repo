using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class RoleDTO
    {
        public class AddRole
        {
            public string RoleId { get; set; }

            public string RoleName { get; set; }

            [ForeignKey("DepartmentId")]
            public string DepartmentId { get; set; }

            public string? CreatedBy { get; set; }

            public string? ModifiedBy { get; set; }
        }


    }
}
