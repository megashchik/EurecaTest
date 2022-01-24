namespace Repository
{
    public interface IRepositoryFactory
    {
        BooksRepository GetBooksRepository();
        AuthorsRepository GetAuthorsRepository();
    }
}
