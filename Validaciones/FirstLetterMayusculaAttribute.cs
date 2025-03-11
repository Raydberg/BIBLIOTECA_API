using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.Validaciones
{
    public class FirstLetterMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid (object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success; //-> para que pase a la validacion de [Required]
            }

            var firstLetter = value.ToString()![0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }
            return ValidationResult.Success;
        }
    }
}
