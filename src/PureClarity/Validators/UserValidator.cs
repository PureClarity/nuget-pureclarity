using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PureClarity.Models;

namespace PureClarity.Validators
{
    internal class UserValidator : PCValidationBase
    {
        public static bool IsDOBValid(string dob)
        {
            var valid = dob == null || !string.IsNullOrWhiteSpace(dob);
            if (!string.IsNullOrWhiteSpace(dob))
            {
                //Check format is dd/MM/yyyy
                var dobRegex = new Regex(@"\d\d\/\d\d\/\d\d\d\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                valid = dobRegex.IsMatch(dob);
            }

            return valid;
        }
    }
}