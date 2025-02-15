using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PerformanceCounter _cpuCounter;

        public CpuMetricJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            ICpuMetricsRepository _cpuMetricsRepository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICpuMetricsRepository>();
            //получаем загрузку cpu и время
            float cpuUsage = _cpuCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            //сохраняем в бд
            _cpuMetricsRepository.Create(new CpuMetricDto() { Time = time, Value = (int)cpuUsage });
            return Task.CompletedTask;
        }
    }
}
