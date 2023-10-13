using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using MinimalApi.Services;

namespace MinimalApi.EndPoints
{
    public static class OrdersEndPoints
    {
        public static void MapOrdersEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async (IUserService _UserService) =>
                Results.Ok(await _UserService.GetUsersAsync()))
                .WithTags("Orders");

            app.MapGet("/orders/{id}", async (int id, IUserService _UserService) =>
                Results.Ok(await _UserService.GetUserByIdAsync(id)))
                .WithTags("Orders");

            app.MapDelete("/orders/{id}", async (int id, IUserService _UserService) =>
                await _UserService.DeleteUserAsync(id))
                .WithTags("Orders");

            app.MapPut("/orders/{id}", async (int id, IUserService _UserService) =>
                await _UserService.DeleteUserAsync(id))
                .WithTags("Orders");

            app.MapPost("/orders/{id}", async (int id, IUserService _UserService) =>
                await _UserService.DeleteUserAsync(id))
                .WithTags("Orders");
        }
    }
}
