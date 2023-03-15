using MeetupAPI;
using MeetupAPI.Data.DbDataSource;
using MeetupAPI.Extensions;
using MeetupAPI.Models;
using Microsoft.AspNetCore;

internal class Program
{

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.MigrateDbContext<EventContext>((context, services) =>
        {
            var env = services.GetService<IWebHostEnvironment>();
            var logger = services.GetService<ILogger<EventContextSeed>>();
            new EventContextSeed().SeedAsync(context, env, logger).Wait();
        });
        host.Run();
    }

    public static IWebHostBuilder CreateHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
}