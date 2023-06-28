using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace OnlineShopPoc;

public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
{
    private readonly SmtpConfig _smtpConfig;
    private readonly ILogger<EmailSenderLoggingDecorator> _logger;
    
    public MailKitSmtpEmailSender(IOptionsSnapshot<SmtpConfig> snapshotOptionsAccessor, ILogger<EmailSenderLoggingDecorator> logger)
    {
        ArgumentNullException.ThrowIfNull(snapshotOptionsAccessor);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _smtpConfig = snapshotOptionsAccessor.Value;
    }
    
    private readonly SmtpClient _smtpClient = new();
    public async ValueTask DisposeAsync()
    {
        await _smtpClient.DisconnectAsync(true);
        _smtpClient.Dispose();
    }


    public async Task SendEmailAsync(string recepientEmail, string subject, string body)
    {
        ArgumentNullException.ThrowIfNull(recepientEmail);
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(body);

        var emailMessage = new MimeMessage
        {
            Subject = subject,
            Body = new TextPart(TextFormat.Plain)
            {
                Text = body,
            },
            From = { MailboxAddress.Parse(_smtpConfig.UserName) },
            To = { MailboxAddress.Parse(recepientEmail) },
        };

        await EnsureConnectedAndAuthentificatedAsync();
        await _smtpClient.SendAsync(emailMessage);
        _logger.LogInformation($"Email sent from {_smtpConfig.SendFrom}");
    }

    private async Task EnsureConnectedAndAuthentificatedAsync()
    {
        if (!_smtpClient.IsConnected)
        {
            await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false);
        }
        
        if (!_smtpClient.IsAuthenticated)
            await _smtpClient.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
    }

    // private string? _emailPassword = System.Environment.GetEnvironmentVariable("asp2023pv112@rodion-m.ru");
}