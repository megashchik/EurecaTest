namespace Model
{
    public interface ICrud<T>
    {
        void Add(T entity);
        T Get(int id);
        void Delete(int id);
        void Update(T entity);
    }
}
