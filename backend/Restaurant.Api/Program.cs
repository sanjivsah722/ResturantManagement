using Restaurant.Api.Endpoints;
using Restaurant.Api.Repositories;
using Restaurant.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IRestaurantRepository, InMemoryRestaurantRepository>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRestaurantOperationsService, RestaurantOperationsService>();

var app = builder.Build();

app.UseCors("ReactApp");

app.MapGet("/", () => Results.Ok(new
{
    application = "Restaurant Management API",
    status = "Running",
    endpoints = new[]
    {
        "/api/auth/login",
        "/api/dashboard",
        "/api/tables",
        "/api/menu",
        "/api/orders",
        "/api/reservations"
    }
}));

app.MapAuthEndpoints();
app.MapRestaurantEndpoints();

app.Run();
