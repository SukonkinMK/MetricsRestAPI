using MetricsData;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private readonly PerformanceCounter _ramCounter;
        private readonly IServiceProvider _serviceProvider;
        public RamMetricJob(IServiceProvider serviceProvider)
        {
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            _serviceProvider = serviceProvider;
        }
        public Task Execute(IJobExecutionContext context)
        {
            IRamMetricsRepository _metricsRepository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IRamMetricsRepository>();
            //получаем загрузку cpu и время
            float ramUsage = _ramCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            //сохраняем в бд
            _metricsRepository.Create(new RamMetricDto() { Time = time, Value = (int)ramUsage });
            return Task.CompletedTask;
        }
    }
}
