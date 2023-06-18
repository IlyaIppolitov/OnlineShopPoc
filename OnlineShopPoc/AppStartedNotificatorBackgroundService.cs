namespace OnlineShopPoc;

public class AppStartedNotificatorBackgroundService : BackgroundService
{

    private readonly IEmailSender _emailSender;
    
    public AppStartedNotificatorBackgroundService(IEmailSender emailSender)
    {
        _emailSender =  emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _emailSender.SendEmail("IppolitovIS@yandex.ru", "Приложение запущено", "Приложение запущено");
    }
}