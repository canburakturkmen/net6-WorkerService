using WebHealthCheck;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)

    
    .UseWindowsService() //Configure service as a windows service
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(@"C:\temp\workerservice\LogFile.txt")
    .CreateLogger();

try
{

    Log.Information("Starting up the service...");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service");
    return;
}
finally
{
    Log.CloseAndFlush();
}


