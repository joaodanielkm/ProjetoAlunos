using EM.Dominio.Entidades;
namespace EM.Dominio.Interfaces;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void Adicione(Aluno obj);

    void Remova(int matricula);

    void Atualize(Aluno obj);

    int ObtenhaProximMatricula();
    
    Aluno ObtenhaPorCpf(string cpf);

}
