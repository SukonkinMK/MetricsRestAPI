using MetricsAgent.Converters;
using MetricsAgent.Profiles;
using MetricsAgent.Services;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;

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
                builder.Logging.SetMinimumLevel(LogLevel.Trace);
                builder.Logging.AddNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });
                // Add services to the container.
                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter())); ;

                builder.Services.AddDbContext<MetricsContext>();
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
            catch (Exception exception) // ��������� ���� ����������, � ���� ������ ����������
            {
                // ������������ ���������� � ���
                logger.Error(exception, "Stopped program because of exception");
                // ����������� ����������, ���������� ������ �������
                throw;
            }
            finally
            {
                // ���������� ������ ������
                NLog.LogManager.Shutdown();
            }
        }        
    }
}