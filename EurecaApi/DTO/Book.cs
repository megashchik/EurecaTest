using Model;
using System.Text.Json.Serialization;
using EvricaApi.Tools;
using System.ComponentModel.DataAnnotations;

namespace EvricaApi.DTO
{
    public record Book : IBook
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; } = null!;
        
        [JsonConverter(typeof(TypeMappingConverterWithNotEmptyArrays<IAuthor[], Author[]>))]
        public IAuthor[] Authors { get; set; } = null!;
    }
}
