namespace EM.Domain.Interface;

public interface IRepositorio<T> where T : IEntidade
{

    T? Get(string obj);

    IEnumerable<T> GetAll();


}
