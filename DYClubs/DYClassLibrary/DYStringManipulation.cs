/*Author: Dong Yan
 *section:PROG2230 sec 04
 * student No.: 5944970
 * email: dyan4970 @conestogac.on.ca
 * Purpose: DYCountryController
 * Revision History: on November 22,2020 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DYClassLibrary
{
    public static class DYStringManipulation
    {
        /// <summary>
        /// method to collect digits from string
        /// </summary>
        /// <param name="inputString">input string from user</param>
        /// <returns>return a string containing all digits found the the input string</returns>
        public static string DYExtractDigits(string inputString)
        {
            string digits = "";
            foreach (char item in inputString)
            {
                if (Char.IsDigit(item))
                {
                    digits += item;
                }
            }
            return digits;
        }
        /// <summary>
        /// check post post matches post code regular express patten or not or is null/empty
        /// </summary>
        /// <param name="post">post code</param>
        /// <param name="postRegx">post code express</param>
        /// <returns>return true if post code match post code regular express patthen or is null/empty,
        /// otherwise return false</returns>
        public static bool DYPostalCodelsValid (string post, string postRegx)
        {
            Regex rg = new Regex(postRegx);
            if (rg.IsMatch(post) || String.IsNullOrEmpty(post))
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// Method to capitalise first letter of every word in the string
        /// </summary>
        /// <param name="inputString">input string from user </param>
        /// <returns>return a string contains words with first letter upper case or an empty string</returns>
        public static string DYCapitalize (string inputString)
        {
            string outputString = String.Empty;
            if (inputString == null)
            {
                outputString = "";
            }else
            {
                // Creates a TextInfo based on the "en-US" culture.
                TextInfo txtInfo = new CultureInfo("en-US", false).TextInfo;

                outputString = txtInfo.ToTitleCase(inputString.ToLower().Trim());
            }

            return outputString;
        }

        
    }
}
