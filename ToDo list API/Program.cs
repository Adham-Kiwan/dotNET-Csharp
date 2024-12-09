using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDo_list_API.Endpoints;
using ToDo_list_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];  // Get the secret key



// Database connection string
var connString = builder.Configuration.GetConnectionString("ToDosStore");

// Register services
builder.Services.AddSqlite<UsersContext>(connString);

// Add Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],  // Use the issuer from config
            ValidAudience = jwtSettings["Audience"],  // Use the audience from config
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Migrate database at startup
app.MigrateDb();

// Use Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map API endpoints
app.MapTodosEndpoints();
// Pass the configuration when mapping user endpoints
app.MapUserEndpoints(builder.Configuration);
app.Run();
