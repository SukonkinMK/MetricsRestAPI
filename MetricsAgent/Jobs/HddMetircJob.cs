using MetricsData;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class HddMetircJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly PerformanceCounter _hddCounter;

        public HddMetircJob(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            IHddMetricsRepository _hddMetricsRepository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHddMetricsRepository>();
            float hddUsage = _hddCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            
            _hddMetricsRepository.Create(new HddMetricDto() { Time = time, Value = (int)hddUsage });
            return Task.CompletedTask;
        }
    }
}
