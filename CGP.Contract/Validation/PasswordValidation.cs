using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class PasswordValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string password)
        {
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasSpecialChar = new Regex(@"[!@#$%^&*(),.?\[\]{}|<>]");
            var hasMinimum6Chars = new Regex(@".{6,}");

            if (!hasUpperCase.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }
            if (!hasSpecialChar.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one special character.");
            }
            if (!hasMinimum6Chars.IsMatch(password))
            {
                return new ValidationResult("Password must be at least 6 characters long.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid password format.");
    }
}
