namespace TestApi
{
    internal class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public Author[] Authors { get; set; } = null!;
    }
}
