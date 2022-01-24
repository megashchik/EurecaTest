using Model;

namespace Repository
{
    internal class PaginationRepository<T> : Repository, IPagination<T> where T : class
    {
        public PaginationRepository(Func<BooksContext> getContext) : base(getContext)
        {
        }

        public T[] Get(int page, int pageSize)
        {
            using var context = GetContext();
            var entities = context.Set<T>();
            int skip = page * pageSize;
            return entities.Skip(skip).Take(pageSize).ToArray();
        }
    }
}
