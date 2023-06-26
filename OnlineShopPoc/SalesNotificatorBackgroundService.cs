using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OnlineShopPoc;

public class SalesNotificatorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SalesNotificatorBackgroundService> _logger;

    public SalesNotificatorBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<SalesNotificatorBackgroundService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ??  throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var localServiceProvider = scope.ServiceProvider;
        var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();
        
        var users = new User[]
        {
            new User("IppolitovIS@yandex.ru")
        };

        var sw = Stopwatch.StartNew();
        foreach (var user in users)
        {
            for (var counter = 0; counter <= _maxAttempt; ++counter)
            {
                try
                {
                    sw.Restart();
                    await emailSender.SendEmailAsync(user.Email, "Акции!", "Промо акции!");
                    counter = _maxAttempt;
                    _logger.LogInformation("Email sent to {Email} in {ElapsedMilliseconds}", user.Email,
                        sw.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    if (counter >= _maxAttempt)
                        _logger.LogError(e, "Ошибка отправка сообщения: {Email}", user.Email);
                    else if (counter >= 0)
                        _logger.LogWarning(e, "Повторная отправка сообщения: {Email}, номер {counter}", 
                            user.Email, counter+1);
                }
            }
        }
        
        //Singleton
        // Email sent to IppolitovIS@yandex.ru in 651 ms
        // Email sent to IppolitovIS@yandex.ru in 170 ms
        
        // Scoped
        // Email sent to IppolitovIS@yandex.ru in 502 ms
        // Email sent to IppolitovIS@yandex.ru in 34 ms

    }
    
    private readonly int _maxAttempt = 2;
}

public record User(string Email);