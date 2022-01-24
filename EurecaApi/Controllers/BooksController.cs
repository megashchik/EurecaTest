using Microsoft.AspNetCore.Mvc;
using Model;
using EvricaApi.DTO;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EvricaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        ICrud<IBook> Books { get; init; }
        IPagination<IBook> Pagination { get; init; }

        public BooksController(ICrud<IBook> crud, IPagination<IBook> pagination, IAuthorBooks bookInfo)
        {
            Books = crud;
            Pagination = pagination;
        }

        // GET: api/<BooksController>?page=0&pageSize=10
        [HttpGet]
        public IActionResult Get([FromQuery] PageSettings settings)
        {
            IBook[] authors = Pagination.Get(settings.Page, settings.PageSize);
            return new JsonResult(authors);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IBook books = Books.Get(id);
            return new JsonResult(books);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] Book book)
        {
            Books.Add(book);
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public void Put([FromBody] Book book)
        {
            Books.Update(book);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Books.Delete(id);
        }
    }
}
