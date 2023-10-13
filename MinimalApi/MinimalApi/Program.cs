using Microsoft.AspNetCore.Authentication;
using MinimalApi.EndPoints;
using MinimalApi.Services;


const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddCors(o => o.AddPolicy(name: MyAllowSpecificOrigins,builder =>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

//builder.Services.AddAuthentication("BasicAuthentication")
//                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
//                ("BasicAuthentication", null);
builder.Services.AddAuthorization();


var app = builder.Build();

app.MapUsersEndPoints();
app.MapOrdersEndPoints();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthentication();
app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();

app.Run();
