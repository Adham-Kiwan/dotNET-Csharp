using Microsoft.EntityFrameworkCore;
using ToDo_list_API.Entities;

namespace ToDo_list_API.Data;

public class UsersContext(DbContextOptions<UsersContext> options)
: DbContext(options)
{
    public DbSet<Todos> Todos => Set<Todos>();

    public DbSet<Users> Users => Set<Users>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //this code will be executed before migration
        //intended to fill the users table with dummy data
        modelBuilder.Entity<Users>().HasData(
            new { Id = 1, Name = "Adham", Email = "adhamkiwan@outlook.com" },
            new { Id = 2, Name = "John", Email = "john@outlook.com" },
            new { Id = 3, Name = "Will", Email = "will@outlook.com" } 
        );
    }
}

