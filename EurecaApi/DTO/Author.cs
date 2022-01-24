using Model;
using System.ComponentModel.DataAnnotations;

namespace EvricaApi.DTO
{
    public record Author : IAuthor
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = null!;
    }
}
