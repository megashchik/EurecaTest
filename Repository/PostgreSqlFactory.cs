namespace Repository
{
    public class PostgreSqlFactory : IRepositoryFactory
    {
        public BooksRepository GetBooksRepository()
        {
            return new BooksRepository(() => new PostgreSqlContext());
        }

        public AuthorsRepository GetAuthorsRepository()
        {
            return new AuthorsRepository(() => new PostgreSqlContext());
        }
    }
}
