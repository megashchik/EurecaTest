using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Model;

namespace Repository.DTO
{
    internal record Book : IBook, IRecordId
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [NotMapped]
        public IAuthor[] Authors => AuthorsCollection.ToArray();

        [JsonIgnore]
        public List<AuthorBook> AuthorsOfBook { get; set; } = new List<AuthorBook>();

        [JsonIgnore]
        public ICollection<Author> AuthorsCollection { get; set; } = null!;
    }
}
