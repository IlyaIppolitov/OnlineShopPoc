namespace OnlineShopPoc;

public interface IEmailSender
{
    Task SendEmailAsync(string recepientEmail, string subject, string body);
}