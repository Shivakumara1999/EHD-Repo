using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public string FeedbackType { get; set; }

        public bool? IsActive { get; set; }

    }
}
