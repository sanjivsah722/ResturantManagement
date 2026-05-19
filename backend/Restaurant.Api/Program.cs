using Restaurant.Api;
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

builder.Services.AddSingleton<RestaurantStore>();

var app = builder.Build();

app.UseCors("ReactApp");

app.MapGet("/api/dashboard", (RestaurantStore store) => Results.Ok(store.GetDashboard()));

app.MapGet("/api/tables", (RestaurantStore store) => Results.Ok(store.Tables));

app.MapPatch("/api/tables/{id:int}/status", (int id, UpdateTableStatusRequest request, RestaurantStore store) =>
{
    var table = store.UpdateTableStatus(id, request.Status);
    return table is null ? Results.NotFound() : Results.Ok(table);
});

app.MapGet("/api/menu", (RestaurantStore store) => Results.Ok(store.Menu));

app.MapGet("/api/orders", (RestaurantStore store) => Results.Ok(store.Orders));

app.MapPost("/api/orders", (CreateOrderRequest request, RestaurantStore store) =>
{
    if (request.TableId <= 0 || string.IsNullOrWhiteSpace(request.CustomerName) || request.Items.Count == 0)
    {
        return Results.BadRequest("Table, customer name, and at least one item are required.");
    }

    if (!store.MenuItemsExist(request.Items))
    {
        return Results.BadRequest("One or more menu items were not found.");
    }

    var order = store.CreateOrder(request);
    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapGet("/api/reservations", (RestaurantStore store) => Results.Ok(store.Reservations));

app.Run();
