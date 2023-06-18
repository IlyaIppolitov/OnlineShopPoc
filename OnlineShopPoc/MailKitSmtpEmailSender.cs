using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace OnlineShopPoc;

public class MailKitSmtpEmailSender : IEmailSender
{
    public async Task SendEmail(string recepientEmail, string subject, string body)
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
            From = { MailboxAddress.Parse("asp2022pd011@rodion-m.ru") },
            To = { MailboxAddress.Parse(recepientEmail) },
        };
             
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.beget.com", 25, false);
            await client.AuthenticateAsync("asp2022pd011@rodion-m.ru", _emailPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }

    private string? _emailPassword = System.Environment.GetEnvironmentVariable("asp2022pd011");
}