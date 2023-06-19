using System.Diagnostics;

namespace OnlineShopPoc;

public class AppStartedNotificatorBackgroundService : BackgroundService
{
    private readonly IEmailSender _emailSender;
    
    public AppStartedNotificatorBackgroundService(IEmailSender emailSender)
    { 
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _emailSender.SendEmailAsync("IppolitovIS@yandex.ru", "Приложение запущено", "Приложение запущено");

        // Циклическая отправка сообщения о работоспособности сервиса
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            
            var totalBytesOfMemoryUsed = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            
            await _emailSender.SendEmailAsync("IppolitovIS@yandex.ru", "Приложение работает", $"Приложение потребляет {totalBytesOfMemoryUsed} байт!");
        }
    }
}