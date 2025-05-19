using Demo_API_Input_Validation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Demo_API_Input_Validation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserAuth API", Version = "v1" });
            });

            // Register DbContext with SQLite
            builder.Services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite("Data Source=sqlite.db"));

            var app = builder.Build();

            // Configure the HTTP request pipeline
            
            // Enable Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Ensure SQLite database is created on app start
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}
