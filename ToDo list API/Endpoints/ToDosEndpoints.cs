using System;
using ToDo_list_API.Data;
using ToDo_list_API.DTOs;
using ToDo_list_API.Entities;

namespace ToDo_list_API.Endpoints;

public static class ToDosEndpoints
{
    const string GetToDoEndpointName = "GetTodo";
    private static readonly List<ToDoDto> todos = new List<ToDoDto>()
{
    new ToDoDto(
        1,
        "Task 1",
        "Do homework"
        ),
    new ToDoDto(
        2,
        "Task 2",
        "Download playlist"
        )
};

    public static RouteGroupBuilder MapTodosEndpoints(this WebApplication app)
    {
        // upon validation, the appropriate endpoint filters will be applied
        // and they will recognize the data notations in specified Dtos
        var group = app.MapGroup("/todos").WithParameterValidation();


        //GET all todos
        
        group.MapGet("/", () => todos);


        //GET a todo by id

        group.MapGet("/{id}", (int id) =>
        {
            ToDoDto? todo = todos.Find(todo => todo.Id == id);

            //make sure we return the same type if true or false
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        })
        .WithName(GetToDoEndpointName);


        // Create a new todo 
        // POST /todos
        
        group.MapPost("/", (CreateToDoDto newToDo, UsersContext DbContext ) =>
        {

            Todos ToDo = new()
            {
                Title = newToDo.Title,
                Text = newToDo.Text,
                UserId = 1 // assuming there is a user with id 1
            };
            DbContext.Todos.Add(ToDo);
            DbContext.SaveChanges();

            ToDoDto todoDto = new(
                ToDo.Id,
                ToDo.Title,
                ToDo.Text
            );

            
            return Results.CreatedAtRoute(GetToDoEndpointName, new { id = ToDo.Id }, ToDo);
        });


        // Edit an existing todo
        // PUT /todos

        group.MapPut("/{id}", (int id, UpdateToDoDto updatedToDo) =>
        {

            var index = todos.FindIndex(todo => todo.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            todos[index] = new ToDoDto(
                id,
                updatedToDo.Title,
                updatedToDo.Text
            );
            return Results.NoContent();
        });


        //Delete an existing todo
        //DELETE /todo/id

        group.MapDelete("/{id}", (int id) =>
        {
            todos.RemoveAll(todo => todo.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}

