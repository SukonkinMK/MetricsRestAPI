using MetricsAgent.Converters;
using MetricsAgent.Services;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Data.SQLite;

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
                ConfigureSqlLiteConnection(builder.Services);
                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter())); ;

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
        private static void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100;";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
        }

        private static void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                DropAndCreateTable("cpumetrics", command);
                DropAndCreateTable("dotnetmetrics", command);
                DropAndCreateTable("hddmetrics", command);
                DropAndCreateTable("networkmetrics", command);
                DropAndCreateTable("rammetrics", command);
            }
        }
        private static void DropAndCreateTable(string tableName, SQLiteCommand command)
        {
            command.CommandText = $"DROP TABLE IF EXISTS {tableName}";
            command.ExecuteNonQuery();
            command.CommandText = $"CREATE TABLE {tableName}(id INTEGER PRIMARY KEY, value INT, time INT)";
            command.ExecuteNonQuery();
        }
    }
}