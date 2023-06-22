using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Demo_Booking_Lessons_For_Driving.Models
{
    public class EmployeeLogin
    {
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        [Display(Name = "IsAdmin")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " required")]

        public string IsAdmin { get; set; }
    }
}