namespace SheetImageCompositor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly Generator _generator;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, Generator generator)
    {
        _logger = logger;
        _configuration = configuration;
        _generator = generator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
