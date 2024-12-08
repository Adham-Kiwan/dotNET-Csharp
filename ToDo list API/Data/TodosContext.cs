using Microsoft.EntityFrameworkCore;
using ToDo_list_API.Entities;

namespace ToDo_list_API.Data;

public class TodosContext(DbContextOptions<TodosContext> options)
: DbContext(options)
{
    public DbSet<Todos> Todos => Set<Todos>();

    public DbSet<Users> Users => Set<Users>();

}

