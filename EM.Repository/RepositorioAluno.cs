using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Repository.Extensoes;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace EM.Repository;

public class RepositorioAluno : IRepositorioAluno
{
   public int ObtenhaProximMatricula()
    {
        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();

        cmd.CommandText = "SELECT GEN_ID(GEN_ALUNO, 1) FROM RDB$DATABASE";

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public IEnumerable<Aluno> ObtenhaTodos()
    {
        List<Aluno> alunos = [];

        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();

        cmd.CommandText = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO";

        using FbDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            string cpf = dr.GetCPF("CPF");

            Aluno aluno = new(cpf)
            {
                Matricula = dr.GetInt32("MATRICULA"),
                Nome = dr.GetString("NOME"),
                Sexo = dr.GetSexo("SEXO"),
                Nascimento = dr.GetDateTime("NASCIMENTO")
            };
            alunos.Add(aluno);
        }

        return alunos;
    }

    public void Adicione(Aluno aluno)
    {
        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();

        int matricula = aluno.Matricula == 0 ? ObtenhaProximMatricula() : aluno.Matricula;

        cmd.CommandText = "INSERT INTO ALUNO (MATRICULA, NOME, SEXO, CPF, NASCIMENTO) VALUES (@MATRICULA, @NOME, @SEXO, @CPF, @NASCIMENTO)";

        cmd.Parameters.AddWithValue("@MATRICULA", matricula);
        cmd.Parameters.AddWithValue("@NOME", aluno.Nome);
        cmd.Parameters.AddWithValue("@SEXO", aluno.Sexo);
        cmd.Parameters.AddWithValue("@CPF", aluno.CPF);
        cmd.Parameters.AddWithValue("@NASCIMENTO", aluno.Nascimento);

        cmd.ExecuteNonQuery();
    }

    public void Atualize(Aluno aluno)
    {
        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();

        cmd.CommandText = "UPDATE ALUNO SET NOME = @NOME, SEXO = @SEXO, CPF = @CPF, NASCIMENTO = @NASCIMENTO WHERE MATRICULA = @MATRICULA";

        cmd.Parameters.AddWithValue("@MATRICULA", aluno.Matricula);
        cmd.Parameters.AddWithValue("@NOME", aluno.Nome);
        cmd.Parameters.AddWithValue("@SEXO", aluno.Sexo);
        cmd.Parameters.AddWithValue("@CPF", aluno.CPF);
        cmd.Parameters.AddWithValue("@NASCIMENTO", aluno.Nascimento);

        cmd.ExecuteNonQuery();
    }

    public void Remova(int matricula)
    {
        using var conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();

        cmd.CommandText = "DELETE FROM ALUNO WHERE MATRICULA = @MATRICULA";

        cmd.Parameters.AddWithValue("@MATRICULA", matricula);

        cmd.ExecuteNonQuery();
    }

    public Aluno Obtenha(string matricula)
    {
        if (string.IsNullOrEmpty(matricula))
        {
            return null;
        }

        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();
        cmd.CommandText = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE MATRICULA = @MATRICULA";

        cmd.Parameters.AddWithValue("@MATRICULA", Uteis.ApenasNumeros(matricula));

        FbDataReader dr = cmd.ExecuteReader();

        return dr.Read()
            ? new Aluno(dr.GetCPF("CPF"))
            {
                Matricula = dr.GetInt32("MATRICULA"),
                Nome = dr.GetString("NOME"),
                Sexo = dr.GetSexo("SEXO"),
                Nascimento = dr.GetDateTime("NASCIMENTO"),
            }
            : null;
    }

    public Aluno ObtenhaPorCpf(string cpf)
    {
        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();
        cmd.CommandText = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE CPF = @CPF";

        cmd.Parameters.AddWithValue("@CPF", Uteis.ApenasNumeros(cpf));

        FbDataReader dr = cmd.ExecuteReader();

        return dr.Read()
            ? new Aluno(dr.GetCPF("CPF"))
            {
                Matricula = dr.GetInt32("MATRICULA"),
                Nome = dr.GetString("NOME"),
                Sexo = dr.GetSexo("SEXO"),
                Nascimento = dr.GetDateTime("NASCIMENTO"),
            }
            : null;
    }
}

