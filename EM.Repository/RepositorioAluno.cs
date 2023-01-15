using EM.Domain.ProjetoEM.EM.Domain;

namespace EM.Repository
{
    public interface IAlunoRepository
    {
        Aluno Selecionar(string id);

        int Persistir(Aluno aluno);
        IEnumerable<Aluno> Listar();

        int Excluir(int id);

        int Atualizar(Aluno aluno);

    }

    public class AlunoRepository : IAlunoRepository
    {

        private readonly Contexto _contexto;

        public AlunoRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Aluno> Listar()
        {
            return _contexto.Set<Aluno>();
        }

        public int Persistir(Aluno aluno)
        {
            _contexto.Alunos.Add(aluno);
            return _contexto.SaveChanges();
        }

        public int Atualizar(Aluno aluno)
        {
            _contexto.Update(aluno);
            return _contexto.SaveChanges();
        }

        public int Excluir(int id)
        {
            _contexto.Alunos.Remove(_contexto.Alunos.Find(id));
            return _contexto.SaveChanges();
        }

        public int PesquisarPorNome(string id)
        {
            _contexto.Alunos.Update(_contexto.Alunos.Find(id));
            return _contexto.SaveChanges();
        }

        public int PesquisarPorMatricula(int id)
        {
            _contexto.Alunos.Update(_contexto.Alunos.Find(id));
            return _contexto.SaveChanges();
        }

        public Aluno Selecionar(string id)
        {
            return _contexto.Set<Aluno>().FirstOrDefault(x => x.Matricula == Convert.ToInt32(id));
        }
    }
}