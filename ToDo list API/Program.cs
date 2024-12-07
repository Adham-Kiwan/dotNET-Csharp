using ToDo_list_API.DTOs;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

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
    todos.Find(todo => todo.Id == id));

app.Run();
