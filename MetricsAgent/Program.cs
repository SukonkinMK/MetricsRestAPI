using MetricsAgent.Converters;
using MetricsAgent.Jobs;
using MetricsAgent.Profiles;
using MetricsAgent.Services;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsAgent
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            NLog.Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Debug("init main");
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Logging.AddNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });
                // Add services to the container.
                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter())); ;

                builder.Services.AddDbContext<MetricsContext>(ServiceLifetime.Scoped);
                builder.Services.AddAutoMapper(typeof(MapperProfile));
                builder.Services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
                builder.Services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
                builder.Services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
                builder.Services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
                builder.Services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });
                    c.MapType<TimeSpan>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("00:00:00")
                    });
                });

                //добавляем Кварц
                builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                builder.Services.AddSingleton<IJobFactory, MetricsJobFactory>();
                builder.Services.AddSingleton<CpuMetricJob>();
                builder.Services.AddSingleton<RamMetricJob>();
                builder.Services.AddSingleton<HddMetircJob>();
                builder.Services.AddSingleton<NetworkMetricJob>();
                builder.Services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/5 * * * * ?"));
                builder.Services.AddSingleton(new JobSchedule(typeof(RamMetricJob), "0/5 * * * * ?"));
                builder.Services.AddSingleton(new JobSchedule(typeof(HddMetircJob), "0/5 * * * * ?"));
                builder.Services.AddSingleton(new JobSchedule(typeof(NetworkMetricJob), "0/5 * * * * ?"));
                builder.Services.AddHostedService<QuartzHostedService>();

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
        }        
    }
}