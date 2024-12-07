using ToDo_list_API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapTodosEnpoints();

app.Run();
