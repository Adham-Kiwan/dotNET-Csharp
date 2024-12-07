using ToDo_list_API.DTOs;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetToDoEndpointName = "GetToDo";
List<ToDoDto> todos = new List<ToDoDto>()
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

//GET all todos
app.MapGet("/todos", () => todos);

//GET a todo by id
app.MapGet("/todos/{id}", (int id) =>
{
    ToDoDto? todo = todos.Find(todo => todo.Id == id);

    //make sure we return the same type if true or false
    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
    .WithName(GetToDoEndpointName);

// Create a new todo 
// POST /todos
app.MapPost("/todos", (CreateToDoDto newToDo) =>
{
    ToDoDto ToDo = new(
        todos.Count + 1,
        newToDo.Title,
        newToDo.Text
    );
    todos.Add(ToDo);

    return Results.CreatedAtRoute(GetToDoEndpointName, new { id = ToDo.Id }, ToDo);
});


// Edit an existing todo
// PUT /todos
app.MapPut("/todos/{id}", (int id, UpdateToDoDto updatedToDo) =>
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
app.MapDelete("/todos/{id}", (int id) =>
{
    todos.RemoveAll(todo => todo.Id == id);

    return Results.NoContent();
});

app.Run();
