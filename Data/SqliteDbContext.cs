using Demo_API_Input_Validation.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo_API_Input_Validation.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
