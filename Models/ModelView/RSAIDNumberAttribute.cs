
using Demo_Booking_Lessons_For_Driving.Models.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Demo_Booking_Lessons_For_Driving.Models
{
    public class RSAIDNumber : ValidationAttribute, IClientValidatable
    {
        public RSAIDNumber()
            :base("{ 0} is not a valid South African ID Number")
        {

        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
                if (value == null)
                    return null;
                  IdentityInfo idInfo = new IdentityInfo(value.ToString ());

            if (!idInfo.IsValid  )
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            
         
            return null;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "rsaid";
            yield return rule;
        }
    }
}