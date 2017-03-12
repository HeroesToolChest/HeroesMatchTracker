using System.Globalization;
using System.Windows.Controls;

namespace HeroesMatchData.Core.Validation
{
    public class NotEmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? string.Empty).ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }
}
