using MetricsData;
namespace MetricsAgent.Models.Requests
{
    public class AllRamMetricsResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }
}
