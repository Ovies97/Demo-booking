using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Demo_Booking_Lessons_For_Driving.Models
{
    [MetadataType(typeof(CartMetaData))]
    public partial  class Cart
    {
    }
    public class CartMetaData
    {
        [Display(Name = "Session")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Session is required")]
        public string Session { get; set; }
        [DataType(DataType.Time)]
        public System.DateTime Time { get; set; }

        [Display(Name = "Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Price is required")]
        public string Price { get; set; }
        [RSAIDNumber(ErrorMessage ="Invalid RSA Identity Document ")]
        public double IDNumber { get; set; }
    }
}