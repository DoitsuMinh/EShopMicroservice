// Shoule be using SQL Server/ MySQL with EF core, but i'm using PostgreSQL with Dapper
using Microsoft.AspNetCore;
using Ordering.API;

var builder = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();


var app = builder.Build();


app.Run();