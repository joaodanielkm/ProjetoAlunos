namespace EM.Domain
{
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
            public int Matricula { get; set; }

            [Display(Name = "Nome")]
            [Column("NOME")]
            public string? Nome { get; set; }

            [Display(Name = "Sexo")]
            [Column("SEXO")]
            public int Sexo = 1;

            [Display(Name = "CPF")]
            [Column("CPF")]
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
