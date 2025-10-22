using Microsoft.AspNetCore.RateLimiting;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

bool isServerGC = GCSettings.IsServerGC;
Console.WriteLine($"Server GC is {(isServerGC ? "enabled" : "disabled")}");

// if development environment, use appsettings.Local.json
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
}

// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", opts =>
    {
        opts.Window = TimeSpan.FromSeconds(10);
        opts.PermitLimit = 5;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRateLimiter();

app.MapReverseProxy();

app.Run();
