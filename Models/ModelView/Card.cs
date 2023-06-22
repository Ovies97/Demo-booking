using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Demo_Booking_Lessons_For_Driving.Models
{
    [MetadataType(typeof(CardMetadata))]
    public partial  class Card
    {
    }
    public class CardMetadata
    {
        [RSAIDNumber(ErrorMessage ="ID number does not exist")]
        public long IDNumber { get; set; }
        public double CardNumber { get; set; }
        [Display(Name = "CVV")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "CVV is required")]
        public string CVV { get; set; }

        [Display(Name = "CardOwner")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "CardOwner is required")]
        public string CardOwner { get; set; }
       
        [Display(Name = "ExpireDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:mm-dd-yyyy}")]
        public DateTime ExpireDate { get; set; }
       
        
    }
}