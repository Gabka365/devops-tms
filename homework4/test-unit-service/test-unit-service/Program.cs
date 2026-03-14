using Microsoft.AspNetCore.Mvc;
using test_unit_service.Managers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAppManager, AppManager>();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.MapGet("entities/{id}", async ([FromServices] IAppManager appManager,
    [FromRoute] int id, ILogger<Program> logger) =>
{
    var entity = await appManager.GetByIdAsync(id);

    logger.LogInformation("App Manager worked at {0} with Id {1}", DateTime.Now.ToString(), entity);

    return entity;
});

app.Run();
