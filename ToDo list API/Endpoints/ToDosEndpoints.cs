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
        
        group.MapGet("/", (UsersContext DbContext) => 
        DbContext.Todos
            .Select(todo => todo.ToToDoSummaryDto())
            .AsNoTracking());  //improve optimization


        //GET a todo by id

        group.MapGet("/{id}", (int id, UsersContext DbContext) =>
        {

            Todos? todo = DbContext.Todos.Find(id);

            //make sure we return the same type if true or false
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        })
        .WithName(GetToDoEndpointName);


        // Create a new todo 
        // POST /todos
        
        group.MapPost("/", (CreateToDoDto newToDo, UsersContext DbContext ) =>
        {

            Todos todo = newToDo.ToEntity();
 
            DbContext.Todos.Add(todo);
            DbContext.SaveChanges();

            return Results.CreatedAtRoute(
                GetToDoEndpointName, new { id = todo.Id }, todo.ToDto());
        });


        // Edit an existing todo
        // PUT /todos

        group.MapPut("/{id}", (int id, UpdateToDoDto updatedToDo, UsersContext DbContext) =>
        {

            var existingToDo = DbContext.Todos.Find(id);

            if (existingToDo is null)
            {
                return Results.NotFound();
            }

            DbContext.Entry(existingToDo)
            .CurrentValues
            .SetValues(updatedToDo.ToEntity(id));

            DbContext.SaveChanges();
            
            return Results.NoContent();
        });


        //Delete an existing todo
        //DELETE /todo/id

        group.MapDelete("/{id}", (int id, UsersContext DbContext) =>
        {
            DbContext.Todos
            .Where(todo => todo.Id == id)
            .ExecuteDelete();

            return Results.NoContent();
        });

        return group;
    }
}