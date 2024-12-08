using System;
using Microsoft.EntityFrameworkCore;

namespace ToDo_list_API.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        // this allowes us to execute migrations on startup
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
        dbContext.Database.Migrate();
    }
}

