using Microsoft.AspNetCore.Mvc;
using MvcRestApi.Models;
using MvcRestApi.Services;

namespace MvcRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetUsers(IUserService userService)
        {
            return Results.Ok(await userService.GetUsersAsync());
        }
        [HttpGet("{id}")]
        public async Task<IResult> GetUserById(int id, IUserService userService)
        {
            return Results.Ok(await userService.GetUserByIdAsync(id));
        }
    }
}