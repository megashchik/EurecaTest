using Microsoft.AspNetCore.Mvc;
using Model;

namespace EvricaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorBooksController : ControllerBase
    {
        IAuthorBooks BookInfo { get; set; }
        public AuthorBooksController(IAuthorBooks bookInfo)
        {
            BookInfo = bookInfo;
        }

        [HttpGet("{authorId}")]
        public IActionResult Get(int authorId)
        {
            IBook[] books = BookInfo.GetBooksByAuthor(authorId);
            return new JsonResult(books);
        }
    }
}
