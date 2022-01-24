using System.Text;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.DTO;

namespace Repository
{
    public class AuthorsRepository : ICrud<IAuthor>, IPagination<IAuthor>
    {
        CrudRepository<Author> CrudRepository { get; init; }
        PaginationRepository<Author> PaginationRepository { get; init; }

        Func<BooksContext> GetContext { get; set; }

        internal AuthorsRepository(Func<BooksContext> getContext)
        {
            CrudRepository = new CrudRepository<Author>(getContext);
            PaginationRepository = new PaginationRepository<Author>(getContext);
            GetContext = getContext;
        }

        public void Add(IAuthor element)
        {
            CrudRepository.Add(element.Convert());
        }

        public void Delete(int id)
        {
            using var context = GetContext();
            bool hasBooks = context.Authors.Where(n => n.Id == id).Include(n => n.Books).Single().Books.Count > 0;
            if(hasBooks)
            {
                throw new Model.Exceptions.InvalidEntityException("The entity contains related elements and cannot be deleted");
            }
            CrudRepository.Delete(id);
        }

        public IAuthor Get(int id)
        {
            return CrudRepository.Get(id);
        }

        public void Update(IAuthor entity)
        {
            CrudRepository.Update(entity.Convert());
        }

        public IAuthor[] Get(int page, int pageSize)
        {
            return PaginationRepository.Get(page, pageSize);
        }
    }
}
