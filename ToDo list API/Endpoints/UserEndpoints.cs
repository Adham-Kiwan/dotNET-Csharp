using System.ComponentModel.DataAnnotations;
using ToDo_list_API.Data;
using ToDo_list_API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ToDo_list_API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app, IConfiguration configuration)
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

            // Retrieve the JWT settings from configuration
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];  // Get the secret key

            // Generate JWT Token
            var token = JwtHelper.GenerateJwtToken(user.Id, user.Email, secretKey);

            var userDto = new { user.Id, user.Name, user.Email, Token = token };
            return Results.Ok(userDto);
        })
        .WithTags("User")
        .WithDescription("Logs in a user using their email.");

        return group;
    }
}
