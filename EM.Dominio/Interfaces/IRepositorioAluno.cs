using EM.Dominio.Interface;
using ProjetoEM.EM.Dominio;

namespace EM.Dominio.Interfaces;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void Add(Aluno obj);

    void Remove(Aluno obj);

    void Update(Aluno obj);
}
