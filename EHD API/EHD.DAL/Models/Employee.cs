using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.Models
{
    public class Employee : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Official Mail Id is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string OfficialMailId { get; set; }

       
        public string? AlternateMailId { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression(@"^\d{6,12}$", ErrorMessage = "Contact Number must be between 6 and 12 digits")]
        [StringLength(12, MinimumLength = 6)]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Joining Date is required")]
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        public int Salary { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }

        
        [ForeignKey("RoleId")]
        public string? RoleId { get; set; }

      

        public string? Password { get; set; }

        public int? Otp { get; set; }

    }
}
