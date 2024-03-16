using AuthenticationApiWIthClientDemo.Client.Auth;
using AuthenticationApiWIthClientDemo.Client.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Registrera HttpClient för att kunna göra http-requests till APIet
builder.Services.AddHttpClient("AuthenticationApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7017");
});
//Registrera UserService
builder.Services.AddScoped<UserService>();
//Registrera UserCircuitHandler och AuthService samt CustomAuthStateProvider för att kunna använda dessa i hela applikationen
builder.Services.TryAddEnumerable(
    ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

//Aktivera Authentication och Authorization
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
