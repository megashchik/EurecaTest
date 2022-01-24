using Microsoft.EntityFrameworkCore;
using Repository.DTO;

namespace Repository
{
    internal abstract class BooksContext : DbContext
    {
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;

        public DbSet<AuthorBook> AuthorBooks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.AuthorsCollection)
                .WithMany(a => a.Books)
                .UsingEntity<AuthorBook>(
                j => j.HasOne(ab => ab.Author)
                    .WithMany(a => a.AuthorsOfBook)
                    .HasForeignKey(ab => ab.AuthorId),
                j => j.HasOne(ab => ab.Book)
                    .WithMany(b => b.AuthorsOfBook)
                    .HasForeignKey(ab => ab.BookId),
                j => j.HasKey(j => new { j.AuthorId, j.BookId }));
        }
    }
}
