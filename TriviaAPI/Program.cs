using Microsoft.EntityFrameworkCore;
using TriviaAPI.Models;
using TriviaAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSignalR();

string? cadena = builder.Configuration.GetConnectionString("TriviaConnectionStrings");

builder.Services.AddDbContext<Sistem21TriviaContext>(optionsBuilder =>
optionsBuilder.UseMySql(cadena, ServerVersion.AutoDetect((cadena))));

var app = builder.Build();
app.MapHub<TriviaHub>("/triviaHub");
app.MapControllers();
app.Run();
