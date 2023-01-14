namespace EM.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Core.Objects.DataClasses;
    //using System.Web.Mvc;
    using Utilitarios;

    namespace ProjetoEM.EM.Domain
    {


        [Table("ALUNO")]
        public class Aluno
        {


            [Key]
            [Display(Name = "Matricula")]
            [Column("MATRICULA")]
            [Required(ErrorMessage = "Campo Requerido!")]
            [ExisteMatricula]
            [Range(1,999999999, ErrorMessage = "Matricula invalida!")]
            public int Matricula { get; set; }

            [Display(Name = "Nome")]
            [Column("NOME")]
            [Required(ErrorMessage = "Campo Requerido!")]
            [StringLength(100, MinimumLength = 3)]
            public string? Nome { get; set; }

            [Display(Name = "Sexo")]
            [Column("SEXO")]
            [Required(ErrorMessage = "Campo Requerido!")]
            public int Sexo = 1;

            [Display(Name = "Nascimento")]
            [Column("NASCIMENTO")]
            //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            //[DataType(DataType.Date, ErrorMessage = "Uma data válida deve ser informada!")]
            [Required(ErrorMessage = "Campo Requerido!")]
            [MaxLength(10)]
            [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|          (29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Data invalida")]
            public string Nascimento { get; set; }

            //dataonly

            [Display(Name = "CPF")]
            [Column("CPF")]
            //[MaxLength(14)]
            //[DisplayFormat(DataFormatString = "{0:999.999.999-99}", ApplyFormatInEditMode = true)]
            //[ExisteCPF]
            //[RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF Invalido!")]
            public string? CPF { get; set; }


            public class ExisteMatricula : ValidationAttribute
            {
                protected override ValidationResult IsValid(object value, ValidationContext validationContext)
                {
                    return (Int32)value == 1010
                        ? new ValidationResult(errorMessage: "Matricula em uso!")
                        : ValidationResult.Success;
                }
            }

            public class ExisteCPF : ValidationAttribute
            {
                protected override ValidationResult IsValid(object value, ValidationContext validationContext)
                {
                    return (string)value == "122"
                        ? new ValidationResult(errorMessage: "CPF em uso!")
                        : ValidationResult.Success;
                }
            }

            public override bool Equals(object? obj)
            {
                return obj is Aluno aluno &&
                       Matricula == aluno.Matricula &&
                       Nome == aluno.Nome &&
                       CPF == aluno.CPF &&
                       Nascimento == aluno.Nascimento;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Matricula);
            }

            public override string ToString()
            {
                return Matricula + " - " + Nome;
            }

        }
    }
}
