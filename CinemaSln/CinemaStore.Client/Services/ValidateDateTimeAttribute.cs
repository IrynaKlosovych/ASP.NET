using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Services
{
    public class ValidateDateTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return DateTime.TryParse(value.ToString(), out _);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} має невірний формат дати і часу.";
        }
    }
}
