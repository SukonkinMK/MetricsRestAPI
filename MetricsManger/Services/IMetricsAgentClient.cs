using MetricsData;
using MetricsData.Requests;

namespace MetricsManager.Services
{
    public interface IMetricsAgentClient
    {
        MetricsResponse<CpuMetricDto> GetCpuMetrics(MetricsRequest cpuMetricsRequest);
        MetricsResponse<RamMetricDto> GetRamMetrics(MetricsRequest ramMetricsRequest);
        MetricsResponse<NetworkMetricDto> GetNetworkMetrics(MetricsRequest networkMetricsRequest);
        MetricsResponse<HddMetricDto> GetHddMetrics(MetricsRequest hddMetricsRequest);
    }
}
