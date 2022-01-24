namespace Repository
{
    internal abstract class Repository
    {
        protected Func<BooksContext> GetContext { get; init; }

        internal Repository(Func<BooksContext> getContext)
        {
            GetContext = getContext;
        }
    }
}
