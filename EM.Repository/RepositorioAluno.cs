using Microsoft.Extensions.Configuration;
using FirebirdSql.Data.FirebirdClient;
using Dapper;
using EM.Domain.ProjetoEM.EM.Domain;

namespace EM.Repository
{
    //interface
    public interface IAlunoRepository
    {
        Aluno Selecionar(string id);

        int Persistir(Aluno aluno);
        
        IEnumerable<Aluno> Listar();

        int Excluir(string id);

        int Atualizar(Aluno aluno);

    }

    public class AlunoRepository : IAlunoRepository
    {

        private IConfiguration _configuracoes;
        private string _conexao { get { return _configuracoes.GetConnectionString("firedb"); } }

        public AlunoRepository(IConfiguration config)
        {
            _configuracoes = config;
        }

        public IEnumerable<Aluno> Listar()
        {
            using (var conexao = new FbConnection(_conexao))
            {
                return conexao.Query<Aluno>("SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO");
            }
        }

        public int Persistir(Aluno aluno)
        {

            using (var conexao = new FbConnection(_conexao))
            {
                return conexao.Execute("INSERT INTO ALUNO VALUES (@MATRICULA, @NOME, @SEXO, @CPF, @NASCIMENTO)", new
                {
                    MATRICULA = aluno.Matricula,
                    NOME = aluno.Nome.Trim(),
                    NASCIMENTO = aluno.Nascimento,
                    SEXO = aluno.Sexo,
                    CPF = aluno.CPF
                });
            }
        }

        public int Atualizar(Aluno aluno)
        {
            using (var conexao = new FbConnection(_conexao))
            {
                return conexao.Execute("UPDATE ALUNO SET NOME = @NOME, SEXO = @SEXO, CPF = @CPF, NASCIMENTO = @NASCIMENTO WHERE MATRICULA = @MATRICULA", new
                {
                    MATRICULA = aluno.Matricula,
                    NOME = aluno.Nome.Trim(),
                    NASCIMENTO = aluno.Nascimento,
                    SEXO = aluno.Sexo,
                    CPF = aluno.CPF
                });
            }
        }

        public int Excluir(string id)
        {
            using (var conexao = new FbConnection(_conexao))
            {
                return conexao.Execute("DELETE FROM ALUNO WHERE MATRICULA = @MATRICULA", new
                {
                    MATRICULA = id,
                });
            }
        }

        public Aluno Selecionar(string id)
        {
            using (var conexao = new FbConnection(_conexao))
            {
                return conexao.Query<Aluno>("SELECT MATRICULA, NOME, SEXO, CPF, NASCIMENTO FROM ALUNO WHERE MATRICULA = @MATRICULA", new { MATRICULA = id }).FirstOrDefault();
            }
        }
    }
}
