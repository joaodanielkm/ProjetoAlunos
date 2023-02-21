using EM.Domain;
using EM.Domain.ProjetoEM.EM.Domain;

namespace EM.Repository
{
    public abstract class RepositorioAbstrato<T> where T : IEntidade
    {

        public void Add(T obj)
        {
            Add(obj);
        }

        public T Get(string obj)
        {
            return Get(obj);
        }

        public IEnumerable<T> GetAll()
        {
            return GetAll();
        }

        public void Remove(T obj)
        {
            Remove(obj);
        }

        public void Update(T obj)
        {
            Update(obj);
        }
    }
}