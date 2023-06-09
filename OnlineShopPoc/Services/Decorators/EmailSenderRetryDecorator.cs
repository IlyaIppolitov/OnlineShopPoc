using Polly;
using Polly.Retry;

namespace OnlineShopPoc;

public class EmailSenderRetryDecorator : IEmailSender
{
    private readonly IEmailSender _emailSenderImplementation;
    private readonly ILogger<EmailSenderRetryDecorator> _logger;
    private readonly int _attemptLimit;
    private readonly AsyncRetryPolicy? _policy;
    private readonly CancellationTokenSource _policyCTS = new CancellationTokenSource();

    public EmailSenderRetryDecorator(IEmailSender emailSenderImplementation, 
        ILogger<EmailSenderRetryDecorator> logger,
        IConfiguration configuration)
    {
        _emailSenderImplementation = emailSenderImplementation ?? throw new ArgumentNullException(nameof(emailSenderImplementation));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        _attemptLimit = configuration.GetValue<int>("SalesEmailAttemptCount");
        if (_attemptLimit < 0)
            throw new ArgumentOutOfRangeException("Количество повторений отправки email задано некорректно!");
        
        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync( retryCount: _attemptLimit, 
                                sleepDurationProvider: (retryCount) => TimeSpan.FromSeconds(retryCount * 2),
                                onRetry: (exception, sleepDuration, attemptNumber) => {
                _logger.LogWarning(exception, 
                    "Ошибка при отправке сообшения, повторная отправка номер: {RetryCount}, через {SleepDuration}", attemptNumber, sleepDuration);
            });
        
    }

    public async Task SendEmailAsync(string recepientEmail, string subject, string body)
    {
        var sendResult = await _policy!.ExecuteAndCaptureAsync(
            _ => _emailSenderImplementation.SendEmailAsync(recepientEmail, subject, body), _policyCTS.Token);
        
        if (sendResult.Outcome == OutcomeType.Failure)
        {
            _logger.LogError(sendResult.FinalException, "Ошибка отправки сообщения! Сообщение не было отпаврлено");
        }
    }
    
    /// <summary>
    /// Отмена отправки сообщения
    /// </summary>
    public void CancelRetrySendingImmediately()
    {
        _policyCTS.Cancel();
    }
}