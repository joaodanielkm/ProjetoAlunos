using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Utilitarios;

public class Validation
{
    public static string? UltimoCPFF { get; set; }

    public class ExisteCPF : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
            value is not null ?
                (string)value == UltimoCPFF
                ? new ValidationResult(errorMessage: "CPF em uso!")
                : ValidationResult.Success :
                null;
    }
}
