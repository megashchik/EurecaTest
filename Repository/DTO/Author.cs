using Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repository.DTO
{
    internal record Author : IAuthor, IRecordId
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public List<AuthorBook> AuthorsOfBook { get; set; } = new List<AuthorBook>();

        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = null!;
    }
}
