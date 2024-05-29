using EM.Dominio.Interfaces;

namespace EM.Repository;

[Obsolete("Com o uso da injeção de dependencia, será usado inteface")]
public abstract class RepositorioAbstrato<T> where T : IEntidade
{
    public abstract void Add(T obj);

    public abstract T Get(string obj);

    public abstract IEnumerable<T> GetAll();

    public abstract void Remove(T obj);

    public abstract void Update(T obj);
}
