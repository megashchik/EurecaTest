using EvricaApi;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TestApi
{
    public class BooksTest
    {
        string BookTitle = "War and Peace";
        string BookAuthor = "Tolstoy";

        Author testAuthor = new Author() { Name = "Tolstoy", Id = 1000 };

        [OneTimeSetUp]
        public async Task Setup()
        {
            Client = GetClient();

            await Client.PostAsJsonAsync(authorsPath, testAuthor);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await Client.DeleteAsync($"{authorsPath}/{testAuthor.Id}");
        }

        string booksPath = "https://localhost:7034/api/Books";
        string authorsPath = "https://localhost:7034/api/Authors";
        HttpClient Client { get; set; } = null!;
        HttpClient GetClient()
        {
            var application = new WebApplicationFactory<Program>();
            return application.CreateClient();
        }

        [Test]
        public async Task AddPageAndRemove()
        {
            int page = 0;
            int pageSize = 100;

            Book book = new Book
            {
                Id = 300,
                Title = BookTitle
            };
            // get
            var author = await Client.GetFromJsonAsync<Author>($"{authorsPath}/{testAuthor.Id}");
            book.Authors = new[] { author! };
            //add
            var added = await Client.PostAsJsonAsync(booksPath, book);
            Assert.AreEqual(HttpStatusCode.OK, added.StatusCode);
            // page
            var books = await Client.GetFromJsonAsync<Book[]>($"{booksPath}?page={page}&pageSize={pageSize}");
            Assert.True(books!.Length > 0 && books.Length <= pageSize);
            Assert.True(books.Select(n => n.Title).Contains(book.Title));
            // remove
            var bookToRemove = books.First(n => n.Title == book.Title);
            var removed = await Client.DeleteAsync($"{booksPath}/{bookToRemove.Id}");
            Assert.AreEqual(HttpStatusCode.OK, removed.StatusCode);
        }


        [Test]
        public async Task PutInvalidId()
        {
            int invalidId = 555;

            var book = new Book {
                Id = invalidId,
                Title = BookTitle,
                Authors = new[] { testAuthor }
            };

            var update = await Client.PutAsJsonAsync(booksPath, book);
            Assert.AreEqual(HttpStatusCode.BadRequest, update.StatusCode);

        }

        [Test]
        public async Task PutWithoutId()
        {
            var book = new
            {
                Title = BookTitle,
                Authors = new[] { testAuthor }
            };
            var update = await Client.PutAsJsonAsync(booksPath, book);
            Assert.AreEqual(HttpStatusCode.OK, update.StatusCode);

            var get = await Client.GetFromJsonAsync<Book[]>(booksPath);
            var id = get!.Single(n => n.Title == BookTitle).Id;

            var remove = await Client.DeleteAsync($"{booksPath}/{id}");
            Assert.AreEqual(HttpStatusCode.OK, remove.StatusCode);
        }

        [Test]
        public async Task DeleteInvalidId()
        {
            int invalidId = 555;
            var delete = await Client.DeleteAsync($"{booksPath}/{invalidId}");
            Assert.AreEqual(HttpStatusCode.NotFound, delete.StatusCode);
        }

        [Test]
        public async Task GetInvalidId()
        {
            int invalidId = 555;
            var get = await Client.GetAsync($"{booksPath}/{invalidId}");
            Assert.AreEqual(HttpStatusCode.NotFound, get.StatusCode);
        }

        [Test]
        public async Task PutEmptyAuthors()
        {
            var book = new Book
            {
                Id = 555,
                Title = BookTitle
            };
            var update = await Client.PutAsJsonAsync(booksPath, book);
            Assert.AreEqual(HttpStatusCode.BadRequest, update.StatusCode);
        }

        [Test]
        public async Task PostNonexistentAuthor()
        {
            var book = new Book
            {
                Title = BookTitle,
                Authors = new[] {
                    new Author {
                        Id = 1,
                        Name = BookAuthor
                    }
                }
            };
            var update = await Client.PostAsJsonAsync(booksPath, book);
            Assert.AreEqual(HttpStatusCode.BadRequest, update.StatusCode);
        }


        [Test]
        public async Task PutNonexistentAuthor()
        {
            var book = new Book
            {
                Title = BookTitle,
                Authors = new[] {
                    new Author {
                        Id = 1,
                        Name = BookAuthor
                    }
                }
            };
            var update = await Client.PutAsJsonAsync(booksPath, book);
            Assert.AreEqual(HttpStatusCode.BadRequest, update.StatusCode);
        }


        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        public async Task GetInvalidPage(int page, int pageSize)
        {
            var get = await Client.GetAsync($"{booksPath}?page={page}&pageSize={pageSize}");
            Assert.AreEqual(HttpStatusCode.BadRequest, get.StatusCode);
        }

        [Test]
        public async Task GetOnlyPage()
        {
            int page = 0;
            var get = await Client.GetAsync($"{booksPath}?page={page}");
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);
        }

        [Test]
        public async Task GetOnlyPageSize()
        {
            int pageSize = 10;
            var get = await Client.GetAsync($"{booksPath}?pageSize={pageSize}");
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);
        }

    }
}
