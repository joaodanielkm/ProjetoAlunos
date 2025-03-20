using EM.Dominio.Entidades;
using EM.Dominio.Utilitarios;
using EM.Web.Models;

namespace EM.Web.Conversores;

public static class Conversores
{
    public static Aluno Converta(this AlunoModel model) =>
        new(model.CPF)
        {
            Nome = model.Nome,
            Matricula = model.Matricula,
            Nascimento = Uteis.ConvertaData(model.Nascimento),
            Sexo = model.Sexo
        };

    public static AlunoModel Converta(this Aluno aluno) =>
        new()
        {
            Nome = aluno.Nome,
            CPF = aluno.CPF.CPFNumero,
            Matricula = aluno.Matricula,
            Nascimento = aluno.Nascimento,
            Sexo = aluno.Sexo
        };
}
