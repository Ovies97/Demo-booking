using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Demo_Booking_Lessons_For_Driving.Models.ModelView;
namespace Demo_Booking_Lessons_For_Driving.Models
{
    [MetadataType(typeof(LearnerMetadata))]
    public partial  class Learner
    {
     
    }
    public class LearnerMetadata
    {
        [RSAIDNumber(ErrorMessage = "valid RSA ID Number is required.")]
        public double  IDNumber { get; set; }
        [Display(Name="Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress )]
        public string Email { get; set; }

        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Cell Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cell Number is required")]
        [DataType(DataType.PhoneNumber )]
        public string CellNumber { get; set; }

        [Display(Name = "Testing Center")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Testing Center is required")]
        public string TestingCenter { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date )]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Province")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Province is required")]
        public string Province { get; set; }
    }
}