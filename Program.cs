using System.Security.Claims;
using AuthApi.Data;
using AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=localhost,1433;Database=main;User ID=sa;Password=1q2w3e4r@#$; TrustServerCertificate=True"));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapSwagger();

app.MapGet("/", (ClaimsPrincipal user) => user.Identity!.Name).RequireAuthorization();
app.MapPost("/logout", async (SignInManager<User> signInManager, [FromBody]object empty) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
});
app.MapIdentityApi<User>();
app.Run();

