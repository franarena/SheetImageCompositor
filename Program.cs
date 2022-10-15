using SheetImageCompositor;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<Generator>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
