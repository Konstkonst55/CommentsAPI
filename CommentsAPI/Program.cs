using CommentsAPI.Data;
using CommentsAPI.Models;
using CommentsAPI.Repositories;
using CommentsAPI.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<CommentDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICommentRepository, PostgresCommentRepository>();
builder.Services.AddScoped<CommentService>();

var app = builder.Build();

app.MapGet("/comments", (CommentService service) =>
{
    return Results.Ok(service.GetAll());
});

app.MapGet("/comments/{id:int}", (int id, CommentService service) =>
{
    var comment = service.GetById(id);

    return comment is not null ? Results.Ok(comment) : Results.NotFound();
});

app.MapPost("/comments", (Comment comment, CommentService service) =>
{
    var added = service.Add(comment);

    return Results.Created($"/comments/{added.Id}", added);
});

app.MapPatch("/comments/{id:int}", (int id, Comment updatedFields, CommentService service) =>
{
    var updated = service.Update(id, updatedFields);

    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapDelete("/comments/{id:int}", (int id, CommentService service) =>
{
    return service.Delete(id) ? Results.NoContent() : Results.NotFound();
});

app.UseCors();

app.Run();