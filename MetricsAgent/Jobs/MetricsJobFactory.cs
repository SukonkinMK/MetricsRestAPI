using Quartz;
using Quartz.Spi;

namespace MetricsAgent.Jobs
{
    public class MetricsJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MetricsJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job) {}
    }
}
