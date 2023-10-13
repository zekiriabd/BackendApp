using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MinimalApi.Models;
using MinimalApi.Services;
using MiniValidation;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MinimalApi.EndPoints
{
    public static class UsersEndPoints
    {
        public static void MapUsersEndPoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/users", 
            async (IUserService _UserService , HttpContext context) =>
                {
                    var role = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
                    if (role =="Admin") 
                        return Results.Ok(await _UserService.GetUsersAsync());
                    else
                        return Results.Problem("You are not authorized to access this resource.", statusCode: StatusCodes.Status401Unauthorized);
                }
            )
            .WithTags("Users");




            app.MapGet("/users/{id}", [Authorize] async (int id, IUserService _UserService) =>
                Results.Ok(await _UserService.GetUserByIdAsync(id)))
                .WithTags("Users");

            app.MapDelete("/users/{id}", [Authorize] async (int id, IUserService _UserService) =>
                await _UserService.DeleteUserAsync(id))
                .WithTags("Users");


            app.MapPost("/users/", [Authorize] async ([FromBody] User user, IUserService _UserService) =>
            {
                if (!MiniValidator.TryValidate(user, out var errors))
                {
                    return Results.ValidationProblem(errors);   
                }
                else
                {
                    if (await _UserService.AddUserAsync(user))
                        return Results.Ok();
                    else
                        return Results.Conflict("هذا السجل موجود مسبقا");
                }

            }).WithTags("Users");


            app.MapPut("/users/", [Authorize] async ([FromBody] User user, IUserService _UserService) =>
            {
                if (!MiniValidator.TryValidate(user, out var errors))
                {
                    return Results.ValidationProblem(errors);
                }
                else
                {
                    if(await _UserService.UpdateUserAsync(user))
                        return Results.Ok();
                    else
                        return Results.NotFound("هذا السجل الذي تريد تغييره غير موجود");
                }
            }).WithTags("Users");

        }
    }
}

