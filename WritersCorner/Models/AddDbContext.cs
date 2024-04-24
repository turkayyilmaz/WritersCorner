using Microsoft.EntityFrameworkCore;
using WritersCorner.Data;

namespace WritersCorner.Models;
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = Library.db");
    }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
}
