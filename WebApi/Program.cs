using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// To check response use: curl http://localhost:5285/weatherforecast
app.MapGet("/weatherforecast", async (HttpRequest request, CancellationToken cancellationToken) =>
{
    var lastEventId =
        request.Headers.TryGetValue("Last-Event-ID", out var value)
            ? value.ToString()
            : null;

    if (lastEventId is not null)
    {
        // Handle reconnection logic if needed
    }

    return TypedResults.ServerSentEvents(GetWeatherForecast(cancellationToken));
})
.WithName("GetWeatherForecast");

async IAsyncEnumerable<SseItem<WeatherForecast>> GetWeatherForecast(
    [EnumeratorCancellation] CancellationToken cancellationToken)
{
    while (!cancellationToken.IsCancellationRequested)
    {
        var eventId = DateTimeOffset.UtcNow.ToString("O");

        var forecast = new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]);

        yield return new SseItem<WeatherForecast>(forecast, eventType: "weatherForecast")
        {
            EventId = eventId,
            ReconnectionInterval = TimeSpan.FromMilliseconds(500)
        };

        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
    }
}

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}