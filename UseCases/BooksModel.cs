using Repository;

namespace Model
{
    public class BooksModel : ICrud<IBook>, IPagination<IBook>, IAuthorBooks
    {
        BooksRepository BooksRepository { get; init; }

        AuthorsRepository AuthorsRepository { get; init; }

        public BooksModel()
        {
            IRepositoryFactory factory = new PostgreSqlFactory();

            BooksRepository = factory.GetBooksRepository();

            AuthorsRepository = factory.GetAuthorsRepository();
        }


        public void Add(IBook entity)
        {
            CheckAuthors(entity.Authors);
            BooksRepository.Add(entity);
        }

        public void Delete(int id)
        {
            BooksRepository.Delete(id);
        }

        public IBook Get(int id)
        {
            return BooksRepository.Get(id);
        }

        public IBook[] Get(int page, int pageSize)
        {
            return BooksRepository.Get(page, pageSize);
        }

        public IBook[] GetBooksByAuthor(int authorId)
        {
            return BooksRepository.GetBooksByAuthor(authorId);
        }

        public void Update(IBook entity)
        {
            CheckAuthors(entity.Authors);
            BooksRepository.Update(entity);
        }

        void CheckAuthors(IAuthor[] authors)
        {
            if (!AuthorsAreUnique(authors))
                throw new Exceptions.InvalidEntityException("Authors in the list are repeated");
            foreach (var author in authors)
            {
                Exist(author);
            }
        }

        void Exist(IAuthor author)
        {
            var authorInDb = AuthorsRepository.Get(author.Id);
            if (authorInDb.Name != author.Name)
                throw new Exceptions.InvalidEntityException("Author with such data does not exist");
        }

        bool AuthorsAreUnique(IAuthor[] authors)
        {
            if (authors.Select(n => n.Id).Distinct().Count() == authors.Length)
                return true;
            else
                return false;
        }
    }
}