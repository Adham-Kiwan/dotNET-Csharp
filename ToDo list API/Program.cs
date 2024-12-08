using ToDo_list_API.Endpoints;
using ToDo_list_API.Data;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("ToDosStore");

//register services
builder.Services.AddSqlite<TodosContext>(connString);

var app = builder.Build();

app.MapTodosEnpoints();

app.Run();
