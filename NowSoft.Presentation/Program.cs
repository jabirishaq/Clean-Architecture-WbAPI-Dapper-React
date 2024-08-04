using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NowSoft.Application.Commands.Signup;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.JwtToken;
using NowSoft.Infrastructure.Data;
using NowSoft.Infrastructure.Persistance;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy to allow requests from specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Allow requests from the React app URL
               .AllowAnyHeader()  // Allow all headers
               .AllowAnyMethod(); // Allow all HTTP methods
    });
});

// Configure JWT settings and services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<ICustomJwtTokenGenerator, CustomJwtTokenGenerator>();

// Register Dapper context and user repository for dependency injection
builder.Services.AddSingleton<DapperContext>(); // Singleton ensures a single instance throughout the application lifecycle
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Scoped to create an instance per request

// Register MediatR services and configure assemblies
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(SignupCommandHandler).Assembly));

// Add essential services for building a web API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Disable HTTPS requirement for metadata, useful for development
    options.SaveToken = true; // Save the token for future use
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validate the token's signing key
        IssuerSigningKey = new SymmetricSecurityKey(key), // Use the configured secret key
        ValidateIssuer = true, // Validate the issuer of the token
        ValidIssuer = jwtSettings.Issuer, // Specify the valid issuer
        ValidateAudience = true, // Validate the audience of the token
        ValidAudience = jwtSettings.Audience, // Specify the valid audience
        ValidateLifetime = true // Validate the token's expiration
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger for API documentation
    app.UseSwaggerUI(); // Enable Swagger UI for interactive API testing
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

app.UseCors("AllowSpecificOrigin"); // Enable the configured CORS policy

app.MapControllers(); // Map controller routes

app.Run(); // Run the application
