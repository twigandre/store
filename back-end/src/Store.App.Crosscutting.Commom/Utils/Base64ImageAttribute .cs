using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.Utils
{
    public class Base64ImageAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string base64 || string.IsNullOrWhiteSpace(base64))
            {
                return new ValidationResult("Base64 está vazio.");
            }

            try
            {
                var dataIndex = base64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
                if (dataIndex >= 0)
                    base64 = base64[(dataIndex + 7)..];

                Convert.FromBase64String(base64);
                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult("Formato Base64 inválido.");
            }
        }
    }
}
