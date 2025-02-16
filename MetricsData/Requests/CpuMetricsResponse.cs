namespace MetricsData.Requests
{
    public class CpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
        public int AgentId { get; set; }
    }
}
