using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace AttendenceManagementSystem.Utilities
{
    public class NumericalValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "Value cannot be empty.");

            }

             string g=value.ToString().TrimStart('0');
           

            // Regex to allow only digits (0-9)
            if (!Regex.IsMatch(value.ToString(), @"^\d+$") )
            {
                return new ValidationResult(false, "Only numeric values (0-9) are allowed.");
            }

            if ( value.ToString().TrimStart('0').Length<2 || value.ToString().TrimStart('0').Length > 4 )
            {
                return new ValidationResult(false, "Value length can only be between 2 to 5 except starting )'s.");
            }

            return ValidationResult.ValidResult;
        }
    }



  

}
