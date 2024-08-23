namespace EM.Dominio.Interfaces;

public interface IRepositorio<T> where T : IEntidade
{

    T Obtenha(string obj);

    IEnumerable<T> ObtenhaTodos();

}
