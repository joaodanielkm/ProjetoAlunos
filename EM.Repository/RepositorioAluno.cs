using EM.Domain;
using EM.Domain.Interface;
using EM.Domain.ProjetoEM.EM.Domain;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace EM.Repository;

public class RepositorioAluno : IRepositorioAluno
{
    public IEnumerable<Aluno> GetAll()
    {
        using (Banco.ObtenhaConexao())
        {
            List<Aluno> alunos = new();

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

    public void Add(Aluno aluno)
    {
        using FbConnection conexaoFireBird = Banco.ObtenhaConexao();
        try
        {
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
        catch (FbException fbex)
        {
            throw new Exception(fbex.Message);
        }
        finally
        {
            conexaoFireBird.Dispose();
        }
    }

    public void Update(Aluno aluno)
    {
        using FbConnection conexaoFireBird = Banco.ObtenhaConexao();
        try
        {
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
        catch (FbException fbex)
        {
            throw new Exception(fbex.Message);
        }
        finally
        {
            conexaoFireBird.Dispose();
        }
    }

    public void Remove(Aluno aluno)
    {
        try
        {
            string sql = $"DELETE FROM ALUNO WHERE MATRICULA = {aluno.Matricula}";
            Banco.Comando(sql);
        }
        catch (FbException fbex)
        {
            throw new Exception(fbex.Message);
        }
    }

    public Aluno Get(string matricula)
    {
        if (string.IsNullOrEmpty(matricula))
        {
            return null;
        }

        Aluno alunoObtido = new();

        using FbConnection conexaoFireBird = Banco.ObtenhaConexao();

        string sql = $"SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE MATRICULA = {matricula}";
        DataTable dt = Banco.Comando(sql);

        if (dt.Rows.Count > 0)
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
        return null;
    }
}

