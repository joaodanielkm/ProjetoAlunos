using EM.Domain.ProjetoEM.EM.Domain;

namespace EM.Repository
{
    public interface IAlunoRepository
    {
        Aluno Selecionar(string mat);

        int Persistir(Aluno aluno);
        IEnumerable<Aluno> Listar();

        int Excluir(int mat);

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

        public int Excluir(int mat)
        {
            _contexto.Alunos.Remove(_contexto.Alunos.Find(mat));
            return _contexto.SaveChanges();
        }

        public int PesquisarPorNome(string nome)
        {
            _contexto.Alunos.Update(_contexto.Alunos.Find(nome));
            return _contexto.SaveChanges();
        }

        public int PesquisarPorMatricula(int mat)
        {
            _contexto.Alunos.Update(_contexto.Alunos.Find(mat));
            return _contexto.SaveChanges();
        }

        public Aluno Selecionar(string mat)
        {
            return _contexto.Set<Aluno>().FirstOrDefault(x => x.Matricula == Convert.ToInt32(mat));
        }
    }
}