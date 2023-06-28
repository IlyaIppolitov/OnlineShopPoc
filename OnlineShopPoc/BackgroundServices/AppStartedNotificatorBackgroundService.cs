using System.Diagnostics;

namespace OnlineShopPoc;

public class AppStartedNotificatorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AppStartedNotificatorBackgroundService> _logger;
    
    public AppStartedNotificatorBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<AppStartedNotificatorBackgroundService> logger,
        IHostApplicationLifetime hostApplicationLifetime)
    { 
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ??  throw new ArgumentNullException(nameof(logger));
        hostApplicationLifetime.ApplicationStarted.Register(() => _logger.LogInformation("App Started"));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        {
            var localServiceProvider = scope.ServiceProvider;
            var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();

            await emailSender.SendEmailAsync("IppolitovIS@yandex.ru", "Приложение запущено", "Приложение запущено");
        }

        // Циклическая отправка сообщения о работоспособности сервиса
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            await using var scope2 = _serviceProvider.CreateAsyncScope();
            {
                var localServiceProvider = scope2.ServiceProvider;
                var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();
                var totalBytesOfMemoryUsed = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            
                // await emailSender.SendEmailAsync("IppolitovIS@yandex.ru", "Приложение работает", $"Приложение потребляет {totalBytesOfMemoryUsed} байт!");
            }
        }
    }
}