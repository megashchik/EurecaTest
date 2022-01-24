using Microsoft.AspNetCore.Mvc;
using Model;
using EvricaApi.DTO;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EvricaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        ICrud<IAuthor> Authors { get; init; }
        IPagination<IAuthor> Pagination { get; init; }

        public AuthorsController(ICrud<IAuthor> crud, IPagination<IAuthor> pagination)
        {
            Authors = crud;
            Pagination = pagination;
        }

        // GET: api/<AuthorsController>?page=0&pageSize=10
        [HttpGet]
        public IActionResult Get([FromQuery] PageSettings settings)
        {
            IAuthor[] authors = Pagination.Get(settings.Page, settings.PageSize);
            return new JsonResult(authors);
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var author = Authors.Get(id);
            return new ObjectResult(author);
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public void Post([FromBody] Author value)
        {
            Authors.Add(value);
        }

        // PUT api/<AuthorsController>/5
        [HttpPut]
        public void Put([FromBody] Author value)
        {
            Authors.Update(value);
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Authors.Delete(id);
        }
    }
}
