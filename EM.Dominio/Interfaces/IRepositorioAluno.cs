using EM.Dominio.Entidades;
using EM.Dominio.Interface;

namespace EM.Dominio.Interfaces;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void Add(Aluno obj);

    void Remove(Aluno obj);

    void Update(Aluno obj);
}
