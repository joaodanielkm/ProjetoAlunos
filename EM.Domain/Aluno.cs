namespace EM.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace ProjetoEM.EM.Domain
    {

        [Table("ALUNO")]
        public class Aluno : Validation, IEntidade
        {

            [Key]
            [Display(Name = "Matricula")]
            [Column("MATRICULA")]
            [Required(ErrorMessage = "Matricula Requerida!")]
            //[ExisteMatricula]
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
            [DataType(DataType.Date)]
            public DateTime? Nascimento { get; set; }

            [Display(Name = "CPF")]
            [Column("CPF")]
            [MaxLength(14)]
            [ExisteCPF]
            public string? CPF { get; set; }

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
