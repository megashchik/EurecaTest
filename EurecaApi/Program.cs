using Model;

namespace EvricaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(option =>
            {
                option.Filters.Add(new EvricaApi.Filters.ApiExceptionFilterAttribute());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<ICrud<IAuthor>, AuthorsModel>();
            builder.Services.AddTransient<IPagination<IAuthor>, AuthorsModel>();
            builder.Services.AddTransient<ICrud<IBook>, BooksModel>();
            builder.Services.AddTransient<IPagination<IBook>, BooksModel>();
            builder.Services.AddTransient<IAuthorBooks, BooksModel>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
