using System.Diagnostics;

namespace OnlineShopPoc;

public class AppStartedNotificatorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public AppStartedNotificatorBackgroundService(IServiceProvider serviceProvider)
    { 
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {        
        
        // Первичная отправка сообщения о старте сервиса
        using (var scope = _serviceProvider.CreateScope())
        {
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            await emailSender.SendEmailAsync("IppolitovIS@yandex.ru", "Приложение запущено", "Приложение запущено");
        }

        // Циклическая отправка сообщения о работоспособности сервиса
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            
            var totalBytesOfMemoryUsed = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
        
            using (var scope = _serviceProvider.CreateScope())
            {
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                await emailSender.SendEmailAsync("IppolitovIS@yandex.ru", "Приложение работает", $"Приложение потребляет {totalBytesOfMemoryUsed} байт!");
            }
        }
    }
}