using System.Text;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.DTO;

namespace Repository
{
    public class BooksRepository : ICrud<IBook>, IAuthorBooks, IPagination<IBook>
    {
        CrudRepository<Book> CrudRepository { get; init; }

        Func<BooksContext> GetContext { get; init; }
        internal BooksRepository(Func<BooksContext> getContext)
        {
            CrudRepository = new CrudRepository<Book>(getContext);
            GetContext = getContext;
        }

        public IBook[] GetBooksByAuthor(int authorId)
        {
            using var context = GetContext();
            var entities = context.Set<Book>();
            var books = context.Books.Where(n => n.AuthorsOfBook.Select(m => m.AuthorId).Contains(authorId)).Include(n => n.AuthorsCollection).ToArray();
            if (books.Length == 0)
                throw new Model.Exceptions.EntityNotFoundException("No books found by this author");
            return books;
        }

        public void Add(IBook entity)
        {
            CrudRepository.Add(entity.Convert());
        }

        public IBook Get(int id)
        {
            using var context = GetContext();
            try
            {
                var book = context.Books.Include(n => n.AuthorsCollection).Single(n => n.Id == id);
                return book;
            }
            catch(InvalidOperationException)
            {
                throw new Model.Exceptions.EntityNotFoundException($"Entity with this id was not found");
            }
        }

        public void Delete(int id)
        {
            CrudRepository.Delete(id);
        }

        public void Update(IBook element)
        {
            if(element.Id == default)
                Add(element);
            else
            {
                try
                {
                    var converted = element.Convert();
                    using var context = GetContext();
                    var book = context.Books.Include(n => n.AuthorsCollection).Single(n => n.Id == element.Id);
                    book.AuthorsOfBook = converted.AuthorsOfBook;
                    book.Title = converted.Title;
                    context.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    throw new Model.Exceptions.InvalidEntityException("Entity with this id does not exist and cannot be updated", e);
                }
            }
        }

        public IBook[] Get(int page, int pageSize)
        {
            int skip = page * pageSize;
            using var context = GetContext();
            var books = context.Books.Skip(skip).Take(pageSize).Include(n => n.AuthorsCollection).ToArray();
            return books;
        }
    }
}
