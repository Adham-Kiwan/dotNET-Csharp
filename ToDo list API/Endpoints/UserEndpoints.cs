using System.ComponentModel.DataAnnotations;
using ToDo_list_API.Data; // Assuming DbContext is here
using ToDo_list_API.DTOs;

namespace ToDo_list_API.Endpoints;
using Microsoft.EntityFrameworkCore;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users").WithParameterValidation();

        // Login endpoint
        group.MapPost("/login", async (LoginUserDto loginDto, UsersContext dbContext) =>
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
        {
            return Results.NotFound(new { message = "User not found" });
        }

        var userDto = new User(user.Id, user.Name, user.Email);
        return Results.Ok(userDto);
    })
    .WithTags("User")
    .WithDescription("Logs in a user using their email.");


        return group;
    }
}
