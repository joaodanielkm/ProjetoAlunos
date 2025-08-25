using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Repositorio.Extensoes;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Text;

namespace EM.Repositorio;

public class RepositorioAluno : IRepositorioAluno
{
    private Aluno MapeieParaAluno(FbDataReader dr)
    {
        string cpf = dr.GetStringSafe("CPF");
        return new Aluno(cpf)
        {
            Matricula = dr.GetInt32("MATRICULA"),
            Nome = dr.GetStringSafe("NOME"),
            Sexo = dr.GetSexo("SEXO"),
            Nascimento = dr.GetDateTime("NASCIMENTO")
        };
    }

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
            alunos.Add(MapeieParaAluno(dr));
        }

        return alunos;
    }

    public IEnumerable<Aluno> ObtenhaPor(string matricula, string nome)
    {
        List<Aluno> alunos = [];
        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();

        var sql = new StringBuilder("SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE 1=1");

        if (!string.IsNullOrEmpty(matricula))
        {
            sql.Append(" AND MATRICULA = @MATRICULA");
            cmd.Parameters.AddWithValue("@MATRICULA", matricula);
        }

        if (!string.IsNullOrEmpty(nome))
        {
            sql.Append(" AND NOME LIKE @NOME");
            cmd.Parameters.AddWithValue("@NOME", $"%{nome}%");
        }

        cmd.CommandText = sql.ToString();

        using FbDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            alunos.Add(MapeieParaAluno(dr));
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
        cmd.Parameters.AddWithValue("@CPF", aluno.CPF.ToString());
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
        cmd.Parameters.AddWithValue("@CPF", aluno.CPF.ToString());
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

        using FbDataReader dr = cmd.ExecuteReader();

        return dr.Read() ? MapeieParaAluno(dr) : null;
    }

    public Aluno ObtenhaPorCpf(string cpf)
    {
        using FbConnection conexao = Banco.CrieConexao();
        using FbCommand cmd = conexao.CreateCommand();
        cmd.CommandText = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE CPF = @CPF";

        cmd.Parameters.AddWithValue("@CPF", Uteis.ApenasNumeros(cpf));

        using FbDataReader dr = cmd.ExecuteReader();

        return dr.Read() ? MapeieParaAluno(dr) : null;
    }
}
