namespace Model
{
    public interface IAuthorBooks
    {
        IBook[] GetBooksByAuthor(int authorId);
    }
}
