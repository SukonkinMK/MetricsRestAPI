using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        private readonly ICpuMetricsRepository _cpuMetricsRepository;

        public CpuMetricJob(ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"{DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
