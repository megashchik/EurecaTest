using System.ComponentModel.DataAnnotations;

namespace EvricaApi.DTO
{
    public class PageSettings
    {
        [Range(0, int.MaxValue, ErrorMessage = "Page number must not be negative")]
        public int Page { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page size must not be positive")]
        public int PageSize { get; set; }

        public PageSettings()
        {
            // default values
            Page = 0;
            PageSize = 15;
        }
    }
}
