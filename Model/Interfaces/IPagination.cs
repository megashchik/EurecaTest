namespace Model
{
    public interface IPagination<T>
    {
        T[] Get(int page, int pageSize);
    }
}
