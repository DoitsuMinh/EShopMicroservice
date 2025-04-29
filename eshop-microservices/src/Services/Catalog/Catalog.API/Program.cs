using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
<<<<<<< HEAD
builder.Services.AddMarten(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string is not configured.");
    }

    opts.Connection(connectionString);
}).UseLightweightSessions();
=======
>>>>>>> origin/main

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
<<<<<<< HEAD
=======

>>>>>>> origin/main

app.Run();
