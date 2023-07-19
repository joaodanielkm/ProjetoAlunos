using System.ComponentModel.DataAnnotations;
namespace EM.Domain
{
    public class Validation
    {
        public static int? UltimaMatricula { get; set; }
        public static string? UltimoCPFF { get; set; }
        public class ExisteMatricula : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
                value is not null ?
                    (Int32)value == UltimaMatricula
                    ? new ValidationResult(errorMessage: "Matricula em uso!")
                    : ValidationResult.Success :
                    null;
        }

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
}
