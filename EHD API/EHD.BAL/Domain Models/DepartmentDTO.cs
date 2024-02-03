using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class DepartmentDTO
    { 
      public class AddDepartment
      {

            public string? DepartmentId { get; set; }
            public string DepartmentName { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }
      }

      public class DepartmentEditByActive
      {
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

      }

       public class IsActiveModel
       {
        public List<string> Id { get; set; }
       }
    }
}
