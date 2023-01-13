namespace EM.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Core.Objects.DataClasses;

    namespace ProjetoEM.EM.Domain
    {
        [Table("ALUNO")]
        public class Aluno
        {
            [Key]
            [Display(Name = "Matricula")]
            [Column("MATRICULA")]
            [MaxLength(9)]
            public int Matricula { get; set; }

            [Display(Name = "Nome")]
            [Column("NOME")]
            public string? Nome { get; set; }

            [Display(Name = "Sexo")]
            [Column("SEXO")]
            [Required(ErrorMessage = "Campo obrigatório!")]
            public int Sexo = 0;

            [Display(Name = "CPF")]
            [Column("CPF")]
            [MaxLength(14)]
            //[RegularExpression()]
            public string? CPF { get; set; }

            [Display(Name = "Nascimento")]
            [Column("NASCIMENTO")]
            public DateOnly Nascimento { get; set; }


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
