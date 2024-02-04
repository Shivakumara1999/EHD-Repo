using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.BAL.Domain_Models
{
    public class DesignationDTO
    {
        public class AddDesignation
        {

            public int? DesignationId { get; set; }
            public string Designation { get; set; }

            public string? CreatedBy { get; set; }

            public string? ModifiedBy { get; set; }
        }
        public class DesignationEditByActive
        {
            public int DesignationId { get; set; }

            public string Designation { get; set; }

        }
      
    }
}
