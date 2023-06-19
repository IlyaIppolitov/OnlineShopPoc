using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace OnlineShopPoc;

public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
{
    
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
            From = { MailboxAddress.Parse("asp2023pv112@rodion-m.ru") },
            To = { MailboxAddress.Parse(recepientEmail) },
        };

        await EnsureConnectedAndAuthentificatedAsync();
        await _smtpClient.SendAsync(emailMessage);
    }

    private async Task EnsureConnectedAndAuthentificatedAsync()
    {
        if (!_smtpClient.IsConnected)
            await _smtpClient.ConnectAsync("smtp.beget.com", 25, false);
        
        if (!_smtpClient.IsAuthenticated)
            await _smtpClient.AuthenticateAsync("asp2023pv112@rodion-m.ru", _emailPassword);
    }

    private string? _emailPassword = System.Environment.GetEnvironmentVariable("asp2023pv112@rodion-m.ru");
}