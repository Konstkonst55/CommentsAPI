using CommentsAPI.Data;
using CommentsAPI.Logging;
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

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 
builder.Logging.AddProvider(new LoggerProvider(builder.Services.BuildServiceProvider()));

var app = builder.Build();

app.MapGet("/comments", (CommentService service, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("GET /comments");

        return Results.Ok(service.GetAll());
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "������ ��� ��������� ������������");

        return Results.Problem("��������� ������ ��� ��������� ������������");
    }
});

app.MapGet("/comments/{id:int}", (int id, CommentService service, ILogger<Program> logger) =>
{
    try
    {
        var comment = service.GetById(id);
        logger.LogInformation("GET /comments/{Id}, �������: {Found}", id, comment != null);

        return comment is not null ? Results.Ok(comment) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "������ ��� ��������� ����������� ID: {Id}", id);

        return Results.Problem("������ ��� ��������� �����������");
    }
});

app.MapPost("/comments", (Comment comment, CommentService service, ILogger<Program> logger) =>
{
    try
    {
        var added = service.Add(comment);
        logger.LogInformation("POST /comments, ������ ID: {Id}", added.Id);

        return Results.Created($"/comments/{added.Id}", added);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "������ ��� �������� �����������");

        return Results.Problem("������ ��� �������� �����������");
    }
});

app.MapPatch("/comments/{id:int}", (int id, Comment updatedFields, CommentService service, ILogger<Program> logger) =>
{
    try
    {
        var updated = service.Update(id, updatedFields);
        logger.LogInformation("PATCH /comments/{Id}, ���������: {Success}", id, updated != null);

        return updated is not null ? Results.Ok(updated) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "������ ��� ���������� ����������� ID: {Id}", id);

        return Results.Problem("������ ��� ���������� �����������");
    }
});

app.MapDelete("/comments/{id:int}", (int id, CommentService service, ILogger<Program> logger) =>
{
    try
    {
        var deleted = service.Delete(id);
        logger.LogInformation("DELETE /comments/{Id}, �������: {Success}", id, deleted);

        return deleted ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "������ ��� �������� ����������� ID: {Id}", id);

        return Results.Problem("������ ��� �������� �����������");
    }
});


app.MapGet("/logs", (CommentDBContext context) =>
{
    var logs = context.Logs
        .OrderByDescending(l => l.Timestamp)
        .Take(100)
        .ToList();

    return Results.Ok(logs);
});

app.UseCors();

app.Run();