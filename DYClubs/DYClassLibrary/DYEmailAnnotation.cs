/*Author: Dong Yan
 *section:PROG2230 sec 04
 * student No.: 5944970
 * email: dyan4970 @conestogac.on.ca
 * Purpose: DYCountryController
 * Revision History: on November 23,2020 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace DYClassLibrary
{
    public class DYEmailAnnotation : ValidationAttribute
    {
        public DYEmailAnnotation()
        {
            ErrorMessage = "{0} is invalid";
        }
        /// <summary>
        /// customer annotation to validate email
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string emailRegx = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex rg = new Regex(emailRegx);

            if (value == null || value.ToString() == "")
            {
                return ValidationResult.Success;
            }
            else if (rg.IsMatch(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }
        }


    }
}

