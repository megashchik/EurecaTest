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
    public class AuthorBooksTest
    {
        int initId = 300;
        string testName = "Test Name";
        string testTitle = "Test Title";


        string apiPath = "https://localhost:7034/api/AuthorBooks";

        string authorsPath = "https://localhost:7034/api/Authors";

        string booksPath = "https://localhost:7034/api/Books";
        HttpClient Client { get; set; } = null!;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Client = GetClient();

            var author = new Author()
            {
                Id = initId,
                Name = testName
            };

            await Client.PostAsJsonAsync(authorsPath, author);

            var book = new Book()
            {
                Id = initId,
                Title = testTitle,
                Authors = new[] { author }
            };

            await Client.PostAsJsonAsync(booksPath, book);
        }

        [OneTimeTearDown]
        public async Task TeadDown()
        {
            await Client.DeleteAsync($"{booksPath}/{initId}");
            await Client.DeleteAsync($"{authorsPath}/{initId}");
        }
        HttpClient GetClient()
        {
            var application = new WebApplicationFactory<Program>();
            return application.CreateClient();
        }

        [Test]
        public async Task Get()
        {
            var get = await Client.GetFromJsonAsync<Book[]>($"{apiPath}/{initId}");
            var getValue = get!.Single();
            Assert.True(getValue.Title == testTitle);
            Assert.True(getValue.Authors.Single().Name == testName);
        }

        [Test]
        public async Task GetInvalidId()
        {
            int invalidId = 333;
            var getInvalid = await Client.GetAsync($"{apiPath}/{invalidId}");
            Assert.AreEqual(HttpStatusCode.NotFound, getInvalid.StatusCode);
        }
    }
}
