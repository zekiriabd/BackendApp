using MiniApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/users",async (IUserService _UserService) => 
   Results.Ok(await _UserService.GetUsersAsync()));

app.Run();

