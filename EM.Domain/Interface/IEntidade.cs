namespace EM.Domain
{
    public interface IEntidade<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(string obj);
        
        void Add(T obj);
        
        void Remove(T obj);

        void Update(T obj);
    }
}
