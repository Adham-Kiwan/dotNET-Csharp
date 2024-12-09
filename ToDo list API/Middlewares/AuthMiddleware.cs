using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_list_API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token.Replace("Bearer ", "");

                try
                {
                    // Retrieve the secret key from configuration
                    var secretKey = _configuration["JwtSettings:SecretKey"];
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretKey);

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = _configuration["JwtSettings:Issuer"],
                        ValidAudience = _configuration["JwtSettings:Audience"]
                    };

                    // Validate the token and set the user identity
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                    context.User = principal;
                }
                catch (SecurityTokenException ex)
                {
                    // Log the exception (optional)
                    Console.WriteLine($"Token validation failed: {ex.Message}");

                    // Do not reject the request immediately; allow further middleware to handle unauthorized access
                }
            }

            // Proceed to the next middleware or endpoint
            await _next(context);
        }
    }
}
