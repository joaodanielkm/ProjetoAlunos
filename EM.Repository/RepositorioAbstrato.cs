using EM.Domain;

namespace EM.Repository
{
    public abstract class RepositorioAbstrato<T> : IEntidade<T> where T : class
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