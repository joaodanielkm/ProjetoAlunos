using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace EM.Repository;

public class RepositorioAluno : IRepositorioAluno
{
    public IEnumerable<Aluno> ObtenhaTodos()
    {
        using (Banco.ObtenhaConexao())
        {
            List<Aluno> alunos = [];

            string sql = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO";
            DataTable dt = Banco.Comando(sql);

            foreach (DataRow item in dt.Rows)
            {
                Aluno aluno = new()
                {
                    Matricula = item.Field<int>("MATRICULA"),
                    Nome = item.Field<string>("NOME"),
                    Sexo = item.Field<EnumeradorSexo>("SEXO"),
                    CPF = item.Field<string>("CPF"),
                    Nascimento = item.Field<DateTime>("NASCIMENTO")
                };

                alunos.Add(aluno);
            }
            return alunos;
        }
    }

    public void Adicione(Aluno aluno)
    {
        using FbConnection conexaoFireBird = Banco.ObtenhaConexao();
        conexaoFireBird.Open();
        string sql = "INSERT INTO ALUNO (MATRICULA, NOME, SEXO, CPF, NASCIMENTO) VALUES (@MATRICULA, @NOME, @SEXO, @CPF, @NASCIMENTO)";

        FbCommand cmd = new(sql, conexaoFireBird);

        cmd.Parameters.Add("@MATRICULA", SqlDbType.Int);
        cmd.Parameters.Add("@NOME", SqlDbType.VarChar);
        cmd.Parameters.Add("@SEXO", SqlDbType.VarChar);
        cmd.Parameters.Add("@CPF", SqlDbType.VarChar);
        cmd.Parameters.Add("@NASCIMENTO", SqlDbType.DateTime);

        cmd.Parameters["@MATRICULA"].Value = aluno.Matricula;
        cmd.Parameters["@NOME"].Value = aluno.Nome;
        cmd.Parameters["@SEXO"].Value = aluno.Sexo;
        cmd.Parameters["@CPF"].Value = aluno.CPF;
        cmd.Parameters["@NASCIMENTO"].Value = aluno.Nascimento;

        cmd.ExecuteNonQuery();
    }

    public void Atualize(Aluno aluno)
    {
        using FbConnection conexaoFireBird = Banco.ObtenhaConexao();
        conexaoFireBird.Open();
        string sql = "UPDATE ALUNO SET NOME = @NOME, SEXO = @SEXO, CPF = @CPF, NASCIMENTO = @NASCIMENTO WHERE MATRICULA = @MATRICULA";

        FbCommand cmd = new(sql, conexaoFireBird);

        cmd.Parameters.Add("@MATRICULA", SqlDbType.Int);
        cmd.Parameters.Add("@NOME", SqlDbType.VarChar);
        cmd.Parameters.Add("@SEXO", SqlDbType.VarChar);
        cmd.Parameters.Add("@CPF", SqlDbType.VarChar);
        cmd.Parameters.Add("@NASCIMENTO", SqlDbType.DateTime);

        cmd.Parameters["@MATRICULA"].Value = aluno.Matricula;
        cmd.Parameters["@NOME"].Value = aluno.Nome;
        cmd.Parameters["@SEXO"].Value = aluno.Sexo;
        cmd.Parameters["@CPF"].Value = aluno.CPF;
        cmd.Parameters["@NASCIMENTO"].Value = aluno.Nascimento;

        cmd.ExecuteNonQuery();
    }

    public void Remova(Aluno aluno) => Banco.Comando($"DELETE FROM ALUNO WHERE MATRICULA = {aluno.Matricula}");

    public Aluno Obtenha(string matricula)
    {
        if (string.IsNullOrEmpty(matricula))
        {
            return null;
        }

        Aluno alunoObtido = new();

        using FbConnection conexaoFireBird = Banco.ObtenhaConexao();

        string sql = $"SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE MATRICULA = {Uteis.ApenasNumeros(matricula)}";
        DataTable dt = Banco.Comando(sql);

        switch (dt.Rows.Count)
        {
            case > 0:
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Aluno aluno = new()
                        {
                            Matricula = item.Field<int>("MATRICULA"),
                            Nome = item.Field<string>("NOME"),
                            Sexo = item.Field<EnumeradorSexo>("SEXO"),
                            CPF = item.Field<string>("CPF"),
                            Nascimento = item.Field<DateTime>("NASCIMENTO"),
                        };

                        alunoObtido = aluno;
                    }
                    return alunoObtido;
                }

            default:
                return null;
        }
    }
}

