using EM.Dominio.Entidades;

namespace EM.Dominio.Interfaces;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void Adicione(Aluno obj);

    void Remova(Aluno obj);

    void Atualize(Aluno obj);
}
