namespace EM.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace ProjetoEM.EM.Domain
    {


        [Table("ALUNO")]
        public class Aluno
        {

            [Key]
            [Display(Name = "Matricula")]
            [Column("MATRICULA")]
            [Required(ErrorMessage = "Matricula Requerida!")]
            [ExisteMatricula]
            [Range(1, 999999999, ErrorMessage = "Matricula invalida!")]
            public int Matricula { get; set; }

            [Display(Name = "Nome")]
            [Column("NOME")]
            [Required(ErrorMessage = "Nome Requerido!")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "Favor preencher com no minimo 3 e no máximo 100 caracteres!")]
            public string? Nome { get; set; }

            [Display(Name = "Sexo")]
            [Column("SEXO")]
            [Required(ErrorMessage = "Sexo Requerido!")]
            public Sexo Sexo { get; set; }

            [Display(Name = "Nascimento")]
            [Column("NASCIMENTO")]
            [Required(ErrorMessage = "Nascimento Requerido!")]
            //[DisplayFormat(DataFormatString = "dd/mm/yyyy")]
            [DataType(DataType.Date)]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime? Nascimento { get; set; }

            [Display(Name = "CPF")]
            [Column("CPF")]
            [MaxLength(14)]
            [ExisteCPF]
            public string? CPF { get; set; }

            //public Aluno(int matricula, string nome, EnumeradorDeSexo sexo, DateOnly nascimento, string cpf)
            //{
            //    Matricula = matricula;
            //    Nome = nome;
            //    Sexo = sexo;
            //    Nascimento = nascimento;
            //    CPF = cpf;
            //}

            public class ExisteMatricula : ValidationAttribute
            {
                public int UltimaMatricula { get; set; }

                protected override ValidationResult IsValid(object value, ValidationContext validationContext)
                {
                    //fazer consulta no banco e ver se esxite se sim rodar abaixo  
                    return (Int32)value == UltimaMatricula
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
