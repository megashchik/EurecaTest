using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EvricaApi;
using System.Net;
using System.Linq;

namespace TestApi
{
    public class AuthorsTest
    {
        const string name = "Федоров";

        [SetUp]
        public void Setup()
        {
            Client = GetClient();
        }

        string authorsPath = "https://localhost:7034/api/Authors";
        HttpClient Client { get; set; } = null!;
        HttpClient GetClient()
        {
            var application = new WebApplicationFactory<Program>();
            return application.CreateClient();
        }





        [TestCase(1000, null)]
        [TestCase(1000, "")]
        public async Task AddOnlyId(int id, string name)
        {
            Author author = new Author() { Id = id, Name = name };
            var added = await Client.PostAsJsonAsync(authorsPath, author);
            Assert.AreEqual(HttpStatusCode.BadRequest, added.StatusCode);
        }

        [Test]
        public async Task PostInvalidJson()
        {
            var badJson = new { Id = 1000 };
            var added = await Client.PostAsJsonAsync(authorsPath, badJson);
            Assert.AreEqual(HttpStatusCode.BadRequest, added.StatusCode);
        }

        [Test]
        public async Task PutWithoutId()
        {
            var badJson = new { Name = name };
            var posted = await Client.PutAsJsonAsync(authorsPath, badJson);
            Assert.AreEqual(HttpStatusCode.OK, posted.StatusCode);

            var get = await Client.GetFromJsonAsync<Author[]>($"{authorsPath}");
            int id = get!.Single(n => n.Name == name).Id;
            var remove = await Client.DeleteAsync($"{authorsPath}/{id}");
        }

        [Test]
        public async Task PostGetPutAndDelete()
        {
            int id = 1000;

            string newName = "Смирнов";

            Author author = new Author { Name = name, Id = id };
            var add = await Client.PostAsJsonAsync(authorsPath, author);
            Assert.AreEqual(HttpStatusCode.OK, add.StatusCode);

            var get = await Client.GetFromJsonAsync<Author>($"{authorsPath}/{id}");
            Assert.True(get!.Name == name);

            Author toPut = new Author { Name = newName, Id = id };
            var put = await Client.PutAsJsonAsync(authorsPath, toPut);
            Assert.AreEqual(HttpStatusCode.OK, put.StatusCode);

            var getAfterPut = await Client.GetFromJsonAsync<Author>($"{authorsPath}/{id}");
            Assert.True(getAfterPut!.Name == newName);

            var remove = await Client.DeleteAsync($"{authorsPath}/{id}");
            Assert.AreEqual(HttpStatusCode.OK, remove.StatusCode);
        }

        [Test]
        public async Task PostAlredyExistId()
        {

            int id = 1000;

            // normal add
            Author author = new Author { Name = name, Id = id };
            var add = await Client.PostAsJsonAsync(authorsPath, author);
            Assert.AreEqual(HttpStatusCode.OK, add.StatusCode);

            // this id alredy exist
            var badAdd = await Client.PostAsJsonAsync(authorsPath, author);
            Assert.AreEqual(HttpStatusCode.BadRequest, badAdd.StatusCode);


            // remove
            var remove = await Client.DeleteAsync($"{authorsPath}/{id}");
            Assert.AreEqual(HttpStatusCode.OK, remove.StatusCode);
        }

        [Test]
        public async Task PutInvalidId()
        {
            int invalidId = 392;
            Author author = new Author { Id = invalidId, Name = name };
            var putInvalid = await Client.PutAsJsonAsync(authorsPath, author);
            Assert.AreEqual(HttpStatusCode.BadRequest, putInvalid.StatusCode);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        public async Task GetInvalidPage(int page, int pageSize)
        {
            var get = await Client.GetAsync($"{authorsPath}?page={page}&pageSize={pageSize}");
            Assert.AreEqual(HttpStatusCode.BadRequest, get.StatusCode);
        }

        [Test]
        public async Task GetOnlyPage()
        {
            int page = 0;
            var get = await Client.GetAsync($"{authorsPath}?page={page}");
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);
        }

        [Test]
        public async Task GetOnlyPageSize()
        {
            int pageSize = 10;
            var get = await Client.GetAsync($"{authorsPath}?pageSize={pageSize}");
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);
        }
    }
}