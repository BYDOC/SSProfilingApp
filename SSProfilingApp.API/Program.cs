using Microsoft.EntityFrameworkCore;
using SSProfilingApp.Infrastructure.Data;
using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Infrastructure.Services;
using System;
using SSProfilingApp.Infrastructure.Factories;
using SSProfilingApp.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProfilingService, ProfilingService>();
builder.Services.AddScoped<LevenshteinCalculator>();
builder.Services.AddHttpClient<JaroWinklerApiClient>();
builder.Services.AddScoped<ISimilarityCalculatorFactory,SimilarityCalculatorFactory>();
builder.Services.AddScoped<ISimilarityScoreService, SimilarityScoreService>();




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/ping-db", async (AppDbContext db) =>
{
    try
    {
        var count = await db.Individuals.CountAsync();
        return Results.Ok($"DB connection successful. Individuals count: {count}");
    }
    catch (Exception ex)
    {
        return Results.Problem($"DB connection failed: {ex.Message}");
    }
});


app.Run();
