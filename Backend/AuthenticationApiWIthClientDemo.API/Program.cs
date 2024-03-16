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

//Använder in memory databas för att slippa konfigurera en riktig databas
builder.Services.AddDbContext<AuthenticationDbContext>(
     options => options.UseInMemoryDatabase("AppDb"));

//Lägger till Identity och konfigurerar det
builder.Services.AddIdentityApiEndpoints<User>()
     .AddEntityFrameworkStores<AuthenticationDbContext>();

var app = builder.Build();

//Lägger till IdentityApi-endpoints
app.MapIdentityApi<User>();

app.MapGet("/", () => "Hello World!");
app.MapGet("/secure", () => "You are approved!").RequireAuthorization();

app.Run();
