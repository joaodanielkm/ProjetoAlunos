using FirebirdSql.Data.FirebirdClient;
using EM.Domain.ProjetoEM.EM.Domain;
using System.Data;
using EM.Domain;
using EM.Domain.Utilitarios;

namespace EM.Repository
{
    public class AlunoRepository : RepositorioAbstrato<Aluno>, IEntidade<Aluno> 
    {
        FbConnection conexaoFireBird = Banco.Banco.getInstancia().getConexao();

        public IEnumerable<Aluno> GetAll()
        {


            using (conexaoFireBird = Banco.Banco.getInstancia().getConexao())
            {
                List<Aluno> alunos = new List<Aluno>();

                string sql = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO";
                DataTable dt = Banco.Banco.consulta(sql);

                foreach (DataRow item in dt.Rows)
                {
                    Aluno aluno = new Aluno()
                    {
                        Matricula = item.Field<Int32>("MATRICULA"),
                        Nome = item.Field<string>("NOME"),
                        Sexo = item.Field<Sexo>("SEXO"),
                        CPF = item.Field<string>("CPF"),
                        Nascimento = item.Field<DateTime>("NASCIMENTO"),
                    };

                    alunos.Add(aluno);
                }

                return alunos;
            }
        }

        public int Add(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = Banco.Banco.getInstancia().getConexao())
            {

                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "INSERT INTO ALUNO (MATRICULA, NOME, SEXO, CPF, NASCIMENTO) VALUES (@MATRICULA, @NOME, @SEXO, @CPF, @NASCIMENTO)";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

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


                    return cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }

        }

        public int Update(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = Banco.Banco.getInstancia().getConexao())
            {
                Uteis uteis = new Uteis();

                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "UPDATE ALUNO SET NOME = @NOME, SEXO = @SEXO, CPF = @CPF, NASCIMENTO = @NASCIMENTO WHERE MATRICULA = @MATRICULA";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

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


                    return cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        public void Remove(Aluno aluno)
        {
            try
            {
                var sql = $"DELETE FROM ALUNO WHERE MATRICULA = '{aluno.Matricula}'";
                Banco.Banco.consulta(sql);
            }
            catch (FbException fbex)
            {
                throw (fbex);
            }

        }

        public Aluno Get(string id)
        {
            Aluno alunoObtido = new Aluno();
            var mat = id;

            using (FbConnection conexaoFireBird = Banco.Banco.getInstancia().getConexao())
            {

                string sql = "SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE MATRICULA = " + mat;
                DataTable dt = Banco.Banco.consulta(sql);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Aluno aluno = new Aluno()
                        {
                            Matricula = item.Field<Int32>("MATRICULA"),
                            Nome = item.Field<string>("NOME"),
                            Sexo = item.Field<Sexo>("SEXO"),
                            CPF = item.Field<string>("CPF"),
                            Nascimento = item.Field<DateTime>("NASCIMENTO"),
                        };

                        return alunoObtido = aluno;
                    }
                }

                return null;
            }
        }
    }
}

