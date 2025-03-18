namespace EM.Dominio.Interfaces;

public interface IRepositorio<T> where T : IEntidade
{

    T Obtenha(string codigo);

    IEnumerable<T> ObtenhaTodos();

}
