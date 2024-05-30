using Kvadratai.Model;
using Kvadratai.Services.Interfaces;
using Kvadratai.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configure Serilog for file logging.
/// </summary>
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

//Configures logging services to write logs to the console and debug output.
builder.Services.AddLogging(config =>
{
    config.AddConsole(); // Logs to the console
    config.AddDebug();   // Logs to the debug output
});


/// Configures MongoDB client as a singleton service.
/// </summary>
/// <param name="sp">The service provider.</param>
/// <returns>MongoDB client instance.</returns>
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
});

/// <summary>
/// Configures MongoDB database instance as a scoped service.
/// </summary>
/// <param name="sp">The service provider.</param>
/// <returns>MongoDB database instance.</returns>
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["DatabaseName"]);
});

// Registers PointsContext and PointsService as scoped services.
builder.Services.AddScoped<PointsContext>();
builder.Services.AddScoped<IPointsService, PointsService>();

// Adds controller services.
builder.Services.AddControllers();

/// <summary>
/// Adds Swagger services for API documentation.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enables Swagger in the development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adds routing middleware.
app.UseRouting();

// Adds authorization middleware.
app.UseAuthorization();

// Enables HTTP request metrics for Prometheus.
app.UseHttpMetrics();

/// <summary>
/// Adds request timeout middleware to the pipeline.
/// </summary>
app.UseMiddleware<RequestTimeoutMiddleware>(TimeSpan.FromSeconds(5));

/// <summary>
/// Configures endpoints for the application.
/// </summary>
/// <param name="endpoints">Endpoint route builder.</param>
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Maps controller endpoints
    endpoints.MapMetrics();     // Exposes metrics endpoint
});

// Runs the application.
app.Run();
