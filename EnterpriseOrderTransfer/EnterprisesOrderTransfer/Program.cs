// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Serilog;
using Serilog.Events;
using System.Runtime.CompilerServices;
using EnterprisesOrderTransfer;
using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();




var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
Directory.SetCurrentDirectory(rootPath);
string directoryName = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
var logPath = Path.GetFullPath(directoryName + "/EnterprisesOrderTransfer_ServiceLog.txt");


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(@logPath)
    .CreateLogger();

var services = scope.ServiceProvider;


try
{
    Log.Information("Starting the service.");
    CancellationToken stoppingToken = new CancellationToken();
    services.GetRequiredService<Service>().ExcuteAsync(stoppingToken).Wait();
    Log.Information("Goodbye, this service has been finished.");
    Console.WriteLine("Goodbye, this service has been finished.");

    return;
}
catch (Exception ex)
{
    Log.Fatal("There was a problem starting the service.");
    return;
}
finally
{
    Log.CloseAndFlush();
}



static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContent, services) =>
        {
            services.AddSingleton<IService, Service>();
            services.AddSingleton<Service>();
        });
        //.UseSerilog();
}
