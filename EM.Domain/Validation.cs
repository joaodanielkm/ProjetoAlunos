using System.ComponentModel.DataAnnotations;
namespace EM.Domain
{
    public class Validation
    {
        public static int UltimaMatricula { get; set; }
        public static string? UltimoCPFF { get; set; }
        public class ExisteMatricula : ValidationAttribute
        {
            

            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                return (Int32)value == UltimaMatricula
                    ? new ValidationResult(errorMessage: "Matricula em uso!")
                    : ValidationResult.Success;
            }
        }

        public class ExisteCPF : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                return (string)value == UltimoCPFF
                    ? new ValidationResult(errorMessage: "CPF em uso!")
                    : ValidationResult.Success;
            }
        }
    }
}
