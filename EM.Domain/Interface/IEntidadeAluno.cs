using EM.Domain.ProjetoEM.EM.Domain;

namespace EM.Domain.Interface
{
    public interface IEntidadeAluno : IEntidade<Aluno>
    {
        Aluno Get(string id);

        int Add(Aluno aluno);

        IEnumerable<Aluno> GetAll();

        void Remove(string id);

        int Update(Aluno aluno);


    }
}
