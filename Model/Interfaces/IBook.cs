namespace Model
{
    public interface IBook
    {
        int Id { get; }
        string Title { get; }
        IAuthor[] Authors { get; }
    }
}
