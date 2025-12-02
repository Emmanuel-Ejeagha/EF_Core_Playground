using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyFirstEfCoreApp;

public class AppDbContext : DbContext
{
   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connStr = config.GetConnectionString("DefaultConnection");
        
        optionsBuilder.UseSqlServer(connStr);
    }

    public DbSet<Book> Books { get; set; }
}
