using System.Diagnostics;

namespace OnlineShopPoc;

// Паттерн декоратор
public class EmailSenderLoggingDecorator : IEmailSender // приём: перехват зависимостей
{

    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailSenderLoggingDecorator> _logger;
    private readonly Stopwatch _sw;

    public EmailSenderLoggingDecorator(IEmailSender emailSender, ILogger<EmailSenderLoggingDecorator> logger)
    {
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sw = Stopwatch.StartNew();
    }

    public async Task SendEmailAsync(string recepientEmail, string subject, string body)
    {
        _logger.LogInformation($"Sending email to {recepientEmail}, with {subject}, {body}.");
        _sw.Restart();
        await _emailSender.SendEmailAsync(recepientEmail, subject, body);
        _logger.LogInformation($"Email to {recepientEmail} was sent in {_sw.ElapsedMilliseconds} ms.");
    }
    
    
}