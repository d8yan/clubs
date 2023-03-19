/*Author: Dong Yan
 *section:PROG2230 sec 04
 * student No.: 5944970
 * email: dyan4970 @conestogac.on.ca
 * Purpose: DYCountryController
 * Revision History: on November 23,2020 
 */
using DYClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;



namespace DYClubs.Models
{
    [ModelMetadataType(typeof(DYNameAddressMetadata))]
    public partial class NameAddress:IValidatableObject
    {
        //concatenate first name and last name
        [Display(Name = "Name")]
        public string FullName { get

            {
                if (!String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName))
                {
                    return LastName + ", " + FirstName;
                }
                else if (String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName))
                {
                    return "";
                }
                else if (!String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName))
                {
                    return FirstName;
                }
                else
                {
                    return LastName;
                }
            }
                
        }
        //declare a globle variable
        private  Province province;
       
        /// <summary>
        /// self-validating user input
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //connect to database
            var _context = validationContext.GetService<ClubsContext>();

            //Capitalize first name, last name, company name, streetAddress, city
            FirstName = DYStringManipulation.DYCapitalize(FirstName);
            LastName = DYStringManipulation.DYCapitalize(LastName);
            CompanyName = DYStringManipulation.DYCapitalize(CompanyName);
            StreetAddress = DYStringManipulation.DYCapitalize(StreetAddress);
            City = DYStringManipulation.DYCapitalize(City);


            //trim input string and convert null into empty string
     
            PostalCode = (PostalCode + "").Trim();
            ProvinceCode = (ProvinceCode + "").Trim();
            Email = (Email + "").Trim();
            Phone = (Phone + "").Trim();
            //retrieve digits from phone input
            Phone = DYStringManipulation.DYExtractDigits((Phone + "").Trim());

            //to check if at lease one of first name, last name or comany name is provided or not
            if (FirstName == "" && LastName == "" && CompanyName == "")
            {
                yield return new ValidationResult("at lease one of first name, last name or comany name must be provided",
                                                new[] { nameof(FirstName), nameof(LastName), nameof(CompanyName) });
            }
 
            //throw an exception when fetch province code has error
            try
            {
                 province = _context.Province.Include(c=>c.CountryCodeNavigation).Where(p => p.ProvinceCode.Equals(this.ProvinceCode.ToUpper())).FirstOrDefault();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            // to check if province is found in the database or not
            if (ProvinceCode != "" && province == null)
            {
                 yield return new ValidationResult("province is not found in the database", new[] { nameof(ProvinceCode) });
            }
            // to shift province code to upper case
            else if (province != null)
            {
                ProvinceCode = ProvinceCode.ToUpper();
            }
            // to check if province code is provided to edit postal code
            if (PostalCode != "" && ProvinceCode == "")
            {
                yield return new ValidationResult("province is required to edit postal code", new[] { nameof(ProvinceCode) });
            }
            // to check if postal code doesn't match the postal code patten of the country where input province is belong to
            else if (PostalCode != "" && province != null &&!DYStringManipulation.DYPostalCodelsValid(PostalCode.ToUpper(), province.CountryCodeNavigation.PostalPattern))
            {
                yield return new ValidationResult("postal code is not valid for the given province", new[] { nameof(PostalCode) });
            }

            if (PostalCode != "" && province != null && DYStringManipulation.DYPostalCodelsValid(PostalCode.ToUpper(), province.CountryCodeNavigation.PostalPattern))
            {
                //check if postal code matches first letter of the input province patten in Canada
                if (province.CountryCode =="CA" && !province.FirstPostalLetter.Contains(PostalCode.ToUpper().Substring(0, 1)))
                {
                    yield return new ValidationResult("first letter of postal code is not valid for the given province", new[] { nameof(PostalCode) });
                }
                else if (province.CountryCode == "CA" && PostalCode.Length == 6)
                {
                    PostalCode = PostalCode.ToUpper().Insert(3, " ");
                }
            }
            //to check if all postal information or email is provided
            if (Email == "" && (StreetAddress == "" || City =="" 
                || PostalCode =="" || ProvinceCode ==""))
            {
                yield return new ValidationResult("all postal information is required if email is not provided", new[] { nameof(Email) });
            }
            // to check if phone number is provided
            if (Phone == "")
            {
                yield return new ValidationResult("The Phone field is required", new[] { nameof(Phone) });
            }
            // to check if phone input is 10 digits or not
            else if (DYStringManipulation.DYExtractDigits(Phone) == "")
            {
                yield return new ValidationResult("phone must have exactly 10 digits", new[] { nameof(Phone) });
            }
            else if (DYStringManipulation.DYExtractDigits(Phone) != "" &&
                DYStringManipulation.DYExtractDigits(Phone).Length != 10)
            {
                yield return new ValidationResult("phone must have exactly 10 digits", new[] { nameof(Phone) });
            }
            else
            {
                //reformat phone number
                Phone = string.Format("{0}-{1}-{2}", Phone.Substring(0, 3), Phone.Substring(3, 3), Phone.Substring(6, 4));
            }
           
            yield return ValidationResult.Success;
        }
    }
        public class DYNameAddressMetadata
    {
        [Display(Name = "ID")]
        
        public int NameAddressId { get; set; }     
        [Display(Name = "First Name")]
        
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        
        public string LastName { get; set; }
        [Display(Name = "Company Name")]
        
        public string CompanyName { get; set; }
        [Display(Name = "Street Address")]
        
        public string StreetAddress { get; set; }
        
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        
        public string PostalCode { get; set; }
        [Display(Name = "Province Code")]
        
        public string ProvinceCode { get; set; }
        [DYEmailAnnotation(ErrorMessage ="email is not valid")]
        
        public string Email { get; set; }
        
        public string Phone { get; set; }

    }
}
