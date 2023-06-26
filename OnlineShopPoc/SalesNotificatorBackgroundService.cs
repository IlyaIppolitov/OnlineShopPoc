using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OnlineShopPoc;

public class SalesNotificatorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SalesNotificatorBackgroundService> _logger;
    private readonly int _attemptLimit;

    public SalesNotificatorBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<SalesNotificatorBackgroundService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ??  throw new ArgumentNullException(nameof(logger));
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        _attemptLimit = configuration.GetValue<int>("SalesEmailAttemptCount");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {

        await using var scope = _serviceProvider.CreateAsyncScope();
        var localServiceProvider = scope.ServiceProvider;
        var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();
        var sw = Stopwatch.StartNew();
        
        var users = new User[]
        {
            new User("IppolitovIS@yandex.ru")
        };

        foreach (var user in users)
        {
            await SendEmailWithAttempts(user);
        }
        
        async Task SendEmailWithAttempts(User user)
        {
            for (var attempt = 1; attempt <= _attemptLimit; ++attempt)
            {
                try
                {
                    sw.Restart();
                    await emailSender.SendEmailAsync(user.Email, "Акции!", "Промо акции!");
                    _logger.LogInformation("Email sent to {Email} in {ElapsedMilliseconds}", user.Email,
                        sw.ElapsedMilliseconds);
                    break;
                }
                catch (Exception e) when (attempt < _attemptLimit)
                {
                    _logger.LogWarning(e, "Повторная отправка сообщения: {Email}, номер {counter}",
                        user.Email, attempt);
                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Ошибка отправка сообщения: {Email}", user.Email);
                    await Task.Delay(1000);
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
}

public record User(string Email);