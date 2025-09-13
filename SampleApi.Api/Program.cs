using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SampleApi.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// (Optional) If you plan to use typed binding elsewhere:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Simple health check
app.MapGet("/", () => "OK");

// POST /api/process
app.MapPost("/api/process", async ([FromBody] object _, HttpRequest request, ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("ProcessLogger");

    // Read the raw body for explicit JSON parsing
    string json = await new StreamReader(request.Body).ReadToEndAsync();

    // Parse with Newtonsoft.Json
    JObject obj;
    try
    {
        obj = JObject.Parse(json);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Invalid JSON payload.");
        return Results.BadRequest(new MyResponse(400, "Invalid JSON"));
    }

    // Extract and print the `access` array
    var accessArray = (JArray?)obj["access"];
    if (accessArray != null)
    {
        foreach (var entry in accessArray)
        {
            var ts = entry["timestamp"]?.ToObject<long>() ?? 0L;
            var id = entry["id"]?.ToString() ?? "<null>";
            logger.LogInformation("Access entry => timestamp: {Timestamp}, id: {Id}", ts, id);
            Console.WriteLine($"Access entry => timestamp: {ts}, id: {id}"); // also to stdout
        }
    }
    else
    {
        logger.LogInformation("No 'access' array provided.");
    }

    // Return HTTP 200 with a simple response object
    var response = new MyResponse(200, "Processed successfully");
    return Results.Ok(response);
})
.WithName("Process")
.Produces<MyResponse>(StatusCodes.Status200OK)
.Produces<MyResponse>(StatusCodes.Status400BadRequest);

app.Run();
