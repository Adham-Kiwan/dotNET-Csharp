using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ToDo_list_API.Data;
using ToDo_list_API.Entities;

public class TodosEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TodosEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private void SeedDatabase(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Todos.Add(new Todos { Id = 1, Title = "Test Todo", Text = "Hello" });
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task GetTodos_UnauthenticatedUser_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = null;

        var response = await client.GetAsync("/todos/api/v1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }



    [Fact]
public async Task GetTodos_AuthenticatedUser_ReturnsTodos()
{
    var client = _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            // Remove the Sqlite database configuration to avoid conflicts
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UsersContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add an in-memory database for testing purposes
            services.AddDbContext<UsersContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            var sp = services.BuildServiceProvider();
            SeedDatabase(sp); // Ensure the database is seeded with test data
        });
    }).CreateClient();

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJlbWFpbCI6IndpbGxAb3V0bG9vay5jb20iLCJqdGkiOiI1NGRhMjBjMC02NzRlLTQ2ZTgtYmIxNS03NDI1YWZmZDE4YWUiLCJleHAiOjE3Mzc0MzY4MzYsImlzcyI6InlvdXItYXBwIiwiYXVkIjoieW91ci1hcHAifQ.aDSPi7dJcPmlJU5d84z0wJ81oXXaaiYFesCFj33MTHk");

    var response = await client.GetAsync("/todos/api/v1");
    var responseData = await response.Content.ReadAsStringAsync();

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Contains("Test Todo", responseData);
}

}

