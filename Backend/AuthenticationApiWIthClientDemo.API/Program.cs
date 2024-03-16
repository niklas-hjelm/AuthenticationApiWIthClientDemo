using System.Text;
using AuthenticationApiWIthClientDemo.API.Authentication.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

//Anv�nder in memory databas f�r att slippa konfigurera en riktig databas
builder.Services.AddDbContext<AuthenticationDbContext>(
     options => options.UseInMemoryDatabase("AppDb"));

//L�gger till Identity och konfigurerar det
builder.Services.AddIdentityApiEndpoints<User>()
     .AddEntityFrameworkStores<AuthenticationDbContext>();

var app = builder.Build();

//L�gger till IdentityApi-endpoints
app.MapIdentityApi<User>();

app.MapGet("/", () => "Hello World!");
app.MapGet("/secure", () => "You are approved!").RequireAuthorization();

app.Run();
