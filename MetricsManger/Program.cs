using MetricsManager.Models;
using MetricsManager.Profiles;
using MetricsManager.Services;
using MetricsManager.Services.Implementations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using Polly;
using System.Reflection.Metadata;

NLog.Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Logging.AddNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });
    //builder.Services.AddSingleton<IAgentPool<AgentInfo>, AgentPool>();
    builder.Services.AddDbContext<AgetsContext>(ServiceLifetime.Scoped);
    builder.Services.AddAutoMapper(typeof(MapperProfile));
    builder.Services.AddScoped<IAgetInfoRepository, AgetInfoRepository>();
    builder.Services.AddControllers();
    builder.Services.AddHttpClient(); //iHttpClientFactory
    builder.Services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>().
        AddTransientHttpErrorPolicy( p => p.WaitAndRetryAsync(
            retryCount:3, 
            sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
            onRetry: (exeption,sleepDuration,atemptNumber,context) =>
            {
            }));
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsManager", Version = "v1" });
        c.MapType<TimeSpan>(() => new OpenApiSchema
        {
            Type = "string",
            Example = new OpenApiString("00:00:00")
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception) // Обработка всех исключений, в ходе работы приложения
{
    // Фиксирование исключений в лог
    logger.Error(exception, "Stopped program because of exception");
    // Возбуждение исключения, завершение работы сервиса
    throw;
}
finally
{
    // Завершение работы логера
    NLog.LogManager.Shutdown();
}