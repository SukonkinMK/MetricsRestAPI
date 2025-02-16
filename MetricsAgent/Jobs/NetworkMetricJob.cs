using MetricsData;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PerformanceCounter _networkCounter;

        public NetworkMetricJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            PerformanceCounterCategory pcg = new PerformanceCounterCategory("Network Interface");            
            var instance = pcg.GetInstanceNames()[0];
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instance);
         }

        public Task Execute(IJobExecutionContext context)
        {
            INetworkMetricsRepository _networkMetricsRepository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<INetworkMetricsRepository>();
            float networkUsage = _networkCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            //сохраняем в бд
            _networkMetricsRepository.Create(new NetworkMetricDto() { Time = time, Value = (int)networkUsage });
            return Task.CompletedTask;
        }
    }
}
