using ProjetoEM.EM.Domain;

namespace EM.Domain.Interface;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void Add(Aluno obj);

    void Remove(Aluno obj);

    void Update(Aluno obj);
}
