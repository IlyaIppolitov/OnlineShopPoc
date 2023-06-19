using System.Diagnostics;

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
            new User("IppolitovIS@yandex.ru"),
            new User("IppolitovIS@yandex.ru"),
        };

        var sw = Stopwatch.StartNew();
        foreach (var user in users)
        {
            sw.Restart();
            await emailSender.SendEmailAsync(user.Email, "Акции!", "Промо акции!");
            _logger.LogInformation($"Email sent to {user.Email} in {sw.ElapsedMilliseconds}");
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