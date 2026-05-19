using Restaurant.Api.Contracts.Orders;
using Restaurant.Api.Contracts.Tables;
using Restaurant.Api.Services;

namespace Restaurant.Api.Endpoints;

public static class RestaurantEndpoints
{
    public static IEndpointRouteBuilder MapRestaurantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/dashboard", (IRestaurantOperationsService service) => Results.Ok(service.GetDashboard()));

        group.MapGet("/tables", (IRestaurantOperationsService service) => Results.Ok(service.GetTables()));

        group.MapPatch("/tables/{id:int}/status", (int id, UpdateTableStatusRequest request, IRestaurantOperationsService service) =>
        {
            var table = service.UpdateTableStatus(id, request.Status);
            return table is null ? Results.NotFound() : Results.Ok(table);
        });

        group.MapGet("/menu", (IRestaurantOperationsService service) => Results.Ok(service.GetMenu()));

        group.MapGet("/orders", (IRestaurantOperationsService service) => Results.Ok(service.GetOrders()));

        group.MapPost("/orders", (CreateOrderRequest request, IRestaurantOperationsService service) =>
        {
            var result = service.CreateOrder(request);

            return result.IsSuccess
                ? Results.Created($"/api/orders/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        });

        group.MapGet("/reservations", (IRestaurantOperationsService service) => Results.Ok(service.GetReservations()));

        return app;
    }
}
