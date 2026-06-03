using Api.Database;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs-dotnet");
var slowRequestLogDirectory = Path.Combine(logDirectory, "slow-requests-dotnet-.txt");
var errorLogDirectory = Path.Combine(logDirectory, "errors-dotnet-.txt");
var requestsLogDirectory = Path.Combine(logDirectory, "logs-dotnet-.txt");

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(requestsLogDirectory, rollingInterval: RollingInterval.Day, outputTemplate: "{UtcTimestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Properties.ContainsKey("ErrorLog") && le.Properties["ErrorLog"] is ScalarValue scalarValue && scalarValue.Value is bool errorLog && errorLog)
                    .WriteTo.File(errorLogDirectory, rollingInterval: RollingInterval.Day, outputTemplate: "{UtcTimestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Properties.ContainsKey("ElapsedMilliseconds") && (le.Properties["ElapsedMilliseconds"] as ScalarValue)?.Value is long elapsedMilliseconds && elapsedMilliseconds > 5000)
                    .WriteTo.File(slowRequestLogDirectory, rollingInterval: RollingInterval.Day, outputTemplate: "{UtcTimestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .CreateLogger();

builder.Services.AddHealthChecks();
builder.Host.UseSerilog();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.Configure<DbOptions>(
                builder.Configuration.GetSection(nameof(DbOptions)));

builder.WebHost.UseUrls(new[] { "http://0.0.0.0:8080" });

var app = builder.Build();

app.MapGet("/get/{id}", async ([FromRoute] int id, [FromServices] ICarService carService) =>
{
    var result = await carService.GetCarAsync(id);

    if (result == null) {
        return Results.NotFound();
    }

    return Results.Ok(new 
    { 
        id = result.Value.id, 
        mark = result.Value.mark, 
        model = result.Value.model, 
        yearOfRelease = result.Value.yearOfRelease 
    });
});

app.MapPost("/create/mark/{mark}/model/{model}/yearOfRelease/{yearOfRelease}", 
    async ([FromRoute]string mark, string model, int yearOfRelease, [FromServices] ICarService carService) =>
{
    var result = await carService.CreateCarAsync(mark, model, yearOfRelease);
    if (result == null) {
        return Results.BadRequest();
    }

    return Results.Ok(result);
});

app.MapHealthChecks("/health");

app.UseSerilogRequestLogging();

app.Run();