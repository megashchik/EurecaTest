using Model;

namespace Repository.DTO
{
    internal static class TypeConverter
    {
        public static Book Convert(this IBook book)
        {
            Book result = new Book()
            {
                Id = book.Id,
                Title = book.Title,
                AuthorsOfBook = book.Authors.Select( n => new AuthorBook()
                {
                    AuthorId = n.Id,
                    BookId = book.Id
                }).ToList()
            };
            return result;
        }

        public static Author Convert(this IAuthor author)
        {
            Author result = new Author()
            {
                Id = author.Id,
                Name = author.Name
            };
            return result;
        }
    }
}
