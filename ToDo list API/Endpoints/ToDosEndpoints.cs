using System;
using Microsoft.EntityFrameworkCore;
using ToDo_list_API.Data;
using ToDo_list_API.DTOs;
using ToDo_list_API.Entities;
using ToDo_list_API.Mapping;

namespace ToDo_list_API.Endpoints;

public static class ToDosEndpoints
{
    const string GetToDoEndpointName = "GetTodo";

    public static RouteGroupBuilder MapTodosEndpoints(this WebApplication app)
    {
        // upon validation, the appropriate endpoint filters will be applied
        // and they will recognize the data notations in specified Dtos
        var group = app.MapGroup("/todos").WithParameterValidation();


        //GET all todos

        group.MapGet("/", async (UsersContext DbContext, HttpContext context) =>
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(await DbContext.Todos
                .Select(todo => todo.ToToDoSummaryDto())
                .AsNoTracking() // Improve optimization
                .ToListAsync());
        });



        //GET a todo by id

        group.MapGet("/{id}", async (int id, UsersContext DbContext, HttpContext context) =>
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }

            Todos? todo = await DbContext.Todos.FindAsync(id);

            //make sure we return the same type if true or false
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        })
        .WithName(GetToDoEndpointName);


        // Create a new todo 
        // POST /todos

        group.MapPost("/", async (CreateToDoDto newToDo, UsersContext DbContext, HttpContext context) =>
        {
            // Ensure the user is authenticated
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }

            // Extract the user ID from the claims
            var userIdClaim = context.User.FindFirst("id");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Results.BadRequest("User ID is missing from the token.");
            }

            // Try converting the user ID to an integer
            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.BadRequest("Invalid User ID in token.");
            }

            // Create a new todo and assign the user ID
            var todo = newToDo.ToEntity();
            todo.UserId = userId;

            DbContext.Todos.Add(todo);
            await DbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                GetToDoEndpointName, new { id = todo.Id }, todo.ToDto());
        });



        // Edit an existing todo
        // PUT /todos

        group.MapPut("/{id}", async (int id, UpdateToDoDto updatedToDo, UsersContext DbContext, HttpContext context) =>
        {
            // Ensure the user is authenticated
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }

            // Extract the user ID from the claims
            var userIdClaim = context.User.FindFirst("id");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Results.BadRequest("User ID is missing from the token.");
            }

            var existingToDo = await DbContext.Todos.FindAsync(id);

            if (existingToDo is null)
            {
                return Results.NotFound();
            }

            DbContext.Entry(existingToDo)
            .CurrentValues
            .SetValues(updatedToDo.ToEntity(id));

            await DbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        //Delete an existing todo
        //DELETE /todo/id

        group.MapDelete("/{id}", async (int id, UsersContext DbContext, HttpContext context) =>
        {
            // Ensure the user is authenticated
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }

            // Extract the user ID from the claims
            var userIdClaim = context.User.FindFirst("id"); // Use "id" as per your JWT changes
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Results.BadRequest("User ID is missing from the token.");
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.BadRequest("Invalid User ID in token.");
            }

            // Retrieve the ToDo item
            var existingToDo = await DbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id);
            if (existingToDo == null)
            {
                return Results.NotFound();
            }

            // Check if the logged-in user owns this ToDo item
            if (existingToDo.UserId != userId)
            {
                return Results.Forbid(); // User is not the owner of this ToDo
            }

            // Delete the ToDo item
            DbContext.Todos.Remove(existingToDo);
            await DbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        return group;
    }
}