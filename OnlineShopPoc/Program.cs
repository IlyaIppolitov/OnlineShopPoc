using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShopPoc;
using Sentry;
using Sentry.Extensions.Logging.Extensions.DependencyInjection;
using Serilog;

// Инициализация временного логгера, для контроля процесса загрузки
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger(); //означает, что глобальный логер будет заменен на вариант из Host.UseSerilog
Log.Information("Starting up");

// Попытка сборки билдера
try
{
    var builder = WebApplication.CreateBuilder(args);
    
    
    
    // Подключение Serilog
    builder.Host.UseSerilog((_, conf) =>
    {
        conf
            .WriteTo.Console()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day);
    });

    // await emailService.SendMessage();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Подключение smtpConfig, настраемого из файла конфигураций json
    builder.Services.AddOptions<SmtpConfig>()
        .BindConfiguration("SmtpConfig")
        .ValidateDataAnnotations()
        .ValidateOnStart();
    
    // Подключение SentryConfig, настраемого из файла конфигураций json
    builder.Services.AddOptions<SentryConfig>()
        .BindConfiguration("SentryConfig");

    // Подключение пользовательских секретов
    // builder.Configuration.AddUserSecrets<SmtpConfig>();

    // регистрация зависимостей
    // Singleton
    builder.Services.AddSingleton<ICatalog, InMemoryCatalog>();
    builder.Services.AddSingleton<ICurrentTime, UtcCurrentTime>();

    // Для тестирования скидки
    // builder.Services.AddSingleton<ICurrentTime, MondayTime>();

    // регистрация зависимостей
    // Scoped
    builder.Services.AddScoped<IEmailSender, MailKitSmtpEmailSender>();
    builder.Services.Decorate<IEmailSender, EmailSenderLoggingDecorator>();
    builder.Services.Decorate<IEmailSender, EmailSenderRetryDecorator>();
    builder.Services.AddHostedService<AppStartedNotificatorBackgroundService>();
    // builder.Services.AddHostedService<SalesNotificatorBackgroundService>();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    //RPC   
    app.MapGet("/get_products", GetProducts);
    app.MapPost("/add_product", AddProduct);
    app.MapPost("/update_product", UpdateProduct);
    app.MapPost("/delete_product", DeleteProduct);
    app.MapGet("/get_product", GetProductById);
    app.MapPost("/clear_products", ClearProducts);

    //REST
    app.MapGet("/products/all", GetProducts);
    app.MapPost("/products/new", AddProduct);
    app.MapPut("/products/{productId}", UpdateProductById);
    app.MapDelete("/products/{productId}", DeleteProduct);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Сервер рухнул!");
}
finally
{
    Log.Information("Shut down complete");
    await Log.CloseAndFlushAsync(); //перед выходом дожидаемся пока все логи будут записаны
}

IResult AddProduct(Product product, ICatalog catalog, HttpContext context)
{
    catalog.AddProduct(product);
    // Вернуть альтернативный код ответа
    // Первый способ
    return Results.Created($"/products/{product.Id}", product);
    // Второй способ
    // context.Response.StatusCode = StatusCodes.Status201Created;
    // context.Response.Headers.Add();
}
    
List<Product> GetProducts(ICatalog catalog, ICurrentTime curTime)
{
    return catalog.GetProducts(curTime);
}

Product GetProductById(string productId, ICatalog catalog, ICurrentTime curTime)
{
    return catalog.GetProductById(Guid.Parse(productId), curTime);
}

void DeleteProduct(string productId, ICatalog catalog)
{
    catalog.DeleteProduct(Guid.Parse(productId));
}

void UpdateProduct(Product product, ICatalog catalog)
{
    catalog.UpdateProduct(product);
}

void UpdateProductById(string productId, Product product, ICatalog catalog)
{
    catalog.UpdateProductById(Guid.Parse(productId), product);
}

void ClearProducts(ICatalog catalog)
{
    catalog.Clear();
}
